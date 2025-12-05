using System;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.InteropServices;

// =============================================================================
// LOH (Large Object Heap) Demo
// =============================================================================
// This program intentionally allocates large byte arrays (100 MB chunks) that
// exceed the LOH threshold of 85,000 bytes (~85 KB). All allocations will go
// directly to the Large Object Heap.
//
// Key LOH characteristics:
// - Objects >= 85,000 bytes are allocated on the LOH
// - LOH is only collected during Gen 2 (full) garbage collections
// - LOH is NOT compacted by default (can lead to fragmentation)
// - Use GCSettings.LargeObjectHeapCompactionMode to enable compaction
// =============================================================================

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please specify size, e.g. 10MB or 5GB.");
            return;
        }

        string input = args[0].Trim();
        if (!TryParseSize(input, out long bytesToAllocate))
        {
            Console.WriteLine("Invalid size. Allowed suffixes: KB, MB, GB, TB.");
            return;
        }

        // Explicitly set LOH compaction mode to default (no compaction).
        // We're aware that LOH objects won't be compacted during GC.
        // In production, consider CompactOnce if fragmentation becomes an issue.
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.Default;

        int page = Environment.SystemPageSize; // e.g. 4096 on Linux
        const int chunkSize = 100 * 1024 * 1024; // 100 MB – well above LOH threshold (85 KB)
        var hold = new List<byte[]>();
        long total = 0;

        Console.WriteLine($"Allocating {bytesToAllocate:N0} bytes (~{bytesToAllocate / (1024*1024)} MB). PageSize={page}.");

        try
        {
            while (total < bytesToAllocate)
            {
                long remaining = bytesToAllocate - total;
                int thisChunk = (int)Math.Min(chunkSize, remaining);

                var chunk = new byte[thisChunk];
                hold.Add(chunk);

                // Touch pages so the kernel actually maps them
                uint x = 2463534242; // simple PRNG seed

                for (int i = 0; i < thisChunk; i += page)
                {
                    // xorshift32 – fast pseudo-random per page
                    x ^= x << 13; x ^= x >> 17; x ^= x << 5;

                    int end = Math.Min(thisChunk, i + page);
                    for (int j = i; j < end; j++)
                    {
                        // Fill the entire page with (quasi) random bytes
                        chunk[j] = (byte)(x + (uint)(j - i));
                    }
                }

                // Also touch last byte in case it doesn't end exactly on page boundary
                chunk[thisChunk - 1] = 1;

                total += thisChunk;
                Console.WriteLine($"Committed: {total / (1024 * 1024)} MB");
            }

            // Show memory statistics using .NET built-in APIs
            PrintMemoryStats();

            Console.WriteLine("\nDone. Press Enter to release...");
            Console.ReadLine();
        }
        catch (OutOfMemoryException)
        {
            Console.WriteLine($"OutOfMemory at ~{total / (1024 * 1024)} MB (observe RES).");
            Console.ReadLine();
        }
    }

    static bool TryParseSize(string s, out long bytes)
    {
        bytes = 0;
        s = s.Trim();
        var u = s.ToUpperInvariant();

        long factor = 1;
        string num = u;

        if (u.EndsWith("KB")) { factor = 1L << 10; num = u[..^2]; }
        else if (u.EndsWith("MB")) { factor = 1L << 20; num = u[..^2]; }
        else if (u.EndsWith("GB")) { factor = 1L << 30; num = u[..^2]; }
        else if (u.EndsWith("TB")) { factor = 1L << 40; num = u[..^2]; }

        if (!double.TryParse(num, System.Globalization.NumberStyles.Float,
                             System.Globalization.CultureInfo.InvariantCulture, out var value))
            return false;

        var asLong = (long)(value * factor);
        if (asLong < 0) return false;
        bytes = asLong;
        return true;
    }

    /// <summary>
    /// Prints detailed memory statistics using .NET built-in GC APIs.
    /// </summary>
    static void PrintMemoryStats()
    {
        Console.WriteLine();
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("                    MEMORY STATISTICS (.NET)                   ");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");

        // Total managed memory (optionally force full GC first with 'true')
        long totalManaged = GC.GetTotalMemory(forceFullCollection: false);
        Console.WriteLine($"Total Managed Memory:    {FormatBytes(totalManaged)}");

        // Detailed GC memory info (available since .NET Core 3.0)
        GCMemoryInfo gcInfo = GC.GetGCMemoryInfo();

        Console.WriteLine($"Heap Size:               {FormatBytes(gcInfo.HeapSizeBytes)}");
        Console.WriteLine($"Fragmented Bytes:        {FormatBytes(gcInfo.FragmentedBytes)}");
        Console.WriteLine($"Memory Load (bytes):     {FormatBytes(gcInfo.MemoryLoadBytes)}");
        Console.WriteLine($"Total Available Memory:  {FormatBytes(gcInfo.TotalAvailableMemoryBytes)}");
        Console.WriteLine($"High Memory Load Threshold: {gcInfo.HighMemoryLoadThresholdBytes / (1024.0 * 1024 * 1024):F2} GB");

        Console.WriteLine();
        Console.WriteLine("── Heap Generation Sizes ──────────────────────────────────────");

        // GenerationInfo array: [0]=Gen0, [1]=Gen1, [2]=Gen2, [3]=LOH, [4]=POH
        ReadOnlySpan<GCGenerationInfo> genInfo = gcInfo.GenerationInfo;

        string[] genNames = { "Gen 0 (SOH)", "Gen 1 (SOH)", "Gen 2 (SOH)", "LOH", "POH" };
        for (int i = 0; i < genInfo.Length && i < genNames.Length; i++)
        {
            Console.WriteLine($"  {genNames[i],-14} Size: {FormatBytes(genInfo[i].SizeAfterBytes),-12} " +
                              $"Fragmentation: {FormatBytes(genInfo[i].FragmentationAfterBytes)}");
        }

        Console.WriteLine();
        Console.WriteLine("── GC Collection Counts ───────────────────────────────────────");
        Console.WriteLine($"  Gen 0 collections: {GC.CollectionCount(0)}");
        Console.WriteLine($"  Gen 1 collections: {GC.CollectionCount(1)}");
        Console.WriteLine($"  Gen 2 collections: {GC.CollectionCount(2)} (LOH is collected here)");

        Console.WriteLine();
        Console.WriteLine("── GC Settings ────────────────────────────────────────────────");
        Console.WriteLine($"  LOH Compaction Mode:  {GCSettings.LargeObjectHeapCompactionMode}");
        Console.WriteLine($"  Latency Mode:         {GCSettings.LatencyMode}");
        Console.WriteLine($"  Is Server GC:         {GCSettings.IsServerGC}");

        Console.WriteLine();
        Console.WriteLine("── Process Memory (via Environment) ──────────────────────────");
        Console.WriteLine($"  Working Set (64-bit): {FormatBytes(Environment.WorkingSet)}");

        Console.WriteLine("═══════════════════════════════════════════════════════════════");
    }

    static string FormatBytes(long bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
        int i = 0;
        double size = bytes;
        while (size >= 1024 && i < suffixes.Length - 1)
        {
            size /= 1024;
            i++;
        }
        return $"{size:F2} {suffixes[i]}";
    }
}

