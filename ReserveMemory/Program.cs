using System;
using System.Collections.Generic;
using System.Runtime;
using Spectre.Console;

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
    /// Prints detailed memory statistics using .NET built-in GC APIs with Spectre.Console.
    /// </summary>
    static void PrintMemoryStats()
    {
        // Total managed memory (optionally force full GC first with 'true')
        long totalManaged = GC.GetTotalMemory(forceFullCollection: false);

        // Detailed GC memory info (available since .NET Core 3.0)
        GCMemoryInfo gcInfo = GC.GetGCMemoryInfo();

        // Header
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule("[bold blue]MEMORY STATISTICS (.NET)[/]").RuleStyle("blue"));
        AnsiConsole.WriteLine();

        // Overview Table
        var overviewTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .Title("[bold yellow]Overview[/]")
            .AddColumn(new TableColumn("[grey]Metric[/]").LeftAligned())
            .AddColumn(new TableColumn("[grey]Value[/]").RightAligned());

        overviewTable.AddRow("Total Managed Memory", $"[green]{FormatBytes(totalManaged)}[/]");
        overviewTable.AddRow("Heap Size", $"[green]{FormatBytes(gcInfo.HeapSizeBytes)}[/]");
        overviewTable.AddRow("Fragmented Bytes", $"[yellow]{FormatBytes(gcInfo.FragmentedBytes)}[/]");
        overviewTable.AddRow("Memory Load", $"[cyan]{FormatBytes(gcInfo.MemoryLoadBytes)}[/]");
        overviewTable.AddRow("Total Available Memory", $"[blue]{FormatBytes(gcInfo.TotalAvailableMemoryBytes)}[/]");
        overviewTable.AddRow("High Memory Threshold", $"[blue]{gcInfo.HighMemoryLoadThresholdBytes / (1024.0 * 1024 * 1024):F2} GB[/]");
        overviewTable.AddRow("Working Set (Process)", $"[magenta]{FormatBytes(Environment.WorkingSet)}[/]");

        AnsiConsole.Write(overviewTable);
        AnsiConsole.WriteLine();

        // Heap Generation Table
        // GenerationInfo array: [0]=Gen0, [1]=Gen1, [2]=Gen2, [3]=LOH, [4]=POH
        ReadOnlySpan<GCGenerationInfo> genInfo = gcInfo.GenerationInfo;
        string[] genNames = { "Gen 0 (SOH)", "Gen 1 (SOH)", "Gen 2 (SOH)", "[bold red]LOH[/]", "POH" };
        Color[] genColors = { Color.Green, Color.Green, Color.Green, Color.Red, Color.Blue };

        var heapTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .Title("[bold yellow]Heap Generation Sizes[/]")
            .AddColumn(new TableColumn("[grey]Generation[/]").LeftAligned())
            .AddColumn(new TableColumn("[grey]Size[/]").RightAligned())
            .AddColumn(new TableColumn("[grey]Fragmentation[/]").RightAligned());

        for (int i = 0; i < genInfo.Length && i < genNames.Length; i++)
        {
            var color = genColors[i];
            heapTable.AddRow(
                genNames[i],
                $"[{color}]{FormatBytes(genInfo[i].SizeAfterBytes)}[/]",
                $"[yellow]{FormatBytes(genInfo[i].FragmentationAfterBytes)}[/]"
            );
        }

        AnsiConsole.Write(heapTable);
        AnsiConsole.WriteLine();

        // Visual Bar Chart for heap sizes (if there's meaningful data)
        long maxSize = 0;
        for (int i = 0; i < genInfo.Length && i < genNames.Length; i++)
        {
            if (genInfo[i].SizeAfterBytes > maxSize)
                maxSize = genInfo[i].SizeAfterBytes;
        }

        if (maxSize > 0)
        {
            var barChart = new BarChart()
                .Label("[bold yellow]Heap Size Distribution[/]")
                .CenterLabel()
                .Width(60);

            string[] barLabels = { "Gen 0", "Gen 1", "Gen 2", "LOH", "POH" };
            Color[] barColors = { Color.Green, Color.Lime, Color.Yellow, Color.Red, Color.Blue };

            for (int i = 0; i < genInfo.Length && i < barLabels.Length; i++)
            {
                double valueMB = genInfo[i].SizeAfterBytes / (1024.0 * 1024);
                barChart.AddItem(barLabels[i], valueMB, barColors[i]);
            }

            AnsiConsole.Write(barChart);
            AnsiConsole.WriteLine();
        }

        // GC Collection Counts & Settings in a Grid
        var infoGrid = new Grid()
            .AddColumn()
            .AddColumn();

        // GC Collections Panel
        var collectionsTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .Title("[bold yellow]GC Collections[/]")
            .AddColumn("[grey]Generation[/]")
            .AddColumn(new TableColumn("[grey]Count[/]").RightAligned());

        collectionsTable.AddRow("Gen 0", $"[green]{GC.CollectionCount(0)}[/]");
        collectionsTable.AddRow("Gen 1", $"[yellow]{GC.CollectionCount(1)}[/]");
        collectionsTable.AddRow("Gen 2 [dim](incl. LOH)[/]", $"[red]{GC.CollectionCount(2)}[/]");

        // GC Settings Panel
        var settingsTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .Title("[bold yellow]GC Settings[/]")
            .AddColumn("[grey]Setting[/]")
            .AddColumn("[grey]Value[/]");

        settingsTable.AddRow("LOH Compaction", $"[cyan]{GCSettings.LargeObjectHeapCompactionMode}[/]");
        settingsTable.AddRow("Latency Mode", $"[cyan]{GCSettings.LatencyMode}[/]");
        settingsTable.AddRow("Server GC", GCSettings.IsServerGC ? "[green]Yes[/]" : "[grey]No[/]");

        infoGrid.AddRow(collectionsTable, settingsTable);
        AnsiConsole.Write(infoGrid);

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule().RuleStyle("blue"));
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

