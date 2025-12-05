using System;
using System.Collections.Generic;
using System.Runtime;
using Spectre.Console;

// =============================================================================
// LOH (Large Object Heap) Demo
// =============================================================================
// This program allocates byte arrays to demonstrate LOH vs SOH behavior.
// The --objectSize parameter controls whether objects land in LOH or SOH:
//   - Objects >= 85,000 bytes (~85 KB) → LOH (Large Object Heap)
//   - Objects <  85,000 bytes          → SOH (Small Object Heap)
//
// Key LOH characteristics:
// - LOH is only collected during Gen 2 (full) garbage collections
// - LOH is NOT compacted by default (can lead to fragmentation)
// - Use GCSettings.LargeObjectHeapCompactionMode to enable compaction
// =============================================================================

class Program
{
    const int LOH_THRESHOLD = 85_000; // Objects >= this size go to LOH

    static void Main(string[] args)
    {
        if (args.Length == 0 || args[0] is "-h" or "--help" or "/?" or "help")
        {
            PrintUsage();
            return;
        }

        // Parse arguments
        if (!TryParseArguments(args, out long bytesToAllocate, out int objectSize, out int iterations))
        {
            return;
        }

        // Explicitly set LOH compaction mode to default (no compaction).
        // We're aware that LOH objects won't be compacted during GC.
        // In production, consider CompactOnce if fragmentation becomes an issue.
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.Default;

        int page = Environment.SystemPageSize; // e.g. 4096 on Linux

        // Determine if objects will land in LOH or SOH
        bool isLoh = objectSize >= LOH_THRESHOLD;
        string heapType = isLoh ? "[red]LOH[/]" : "[green]SOH[/]";
        
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule("[bold blue]Memory Allocation[/]").RuleStyle("blue"));
        AnsiConsole.WriteLine();
        
        var configTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[grey]Setting[/]")
            .AddColumn("[grey]Value[/]");
        
        configTable.AddRow("Total to allocate", $"[cyan]{FormatBytes(bytesToAllocate)}[/]");
        configTable.AddRow("Object size", $"[cyan]{FormatBytes(objectSize)}[/]");
        configTable.AddRow("LOH threshold", $"[yellow]{FormatBytes(LOH_THRESHOLD)}[/]");
        configTable.AddRow("Target heap", heapType);
        configTable.AddRow("Iterations", $"[cyan]{iterations}[/]");
        configTable.AddRow("Page size", $"[grey]{page} bytes[/]");
        
        AnsiConsole.Write(configTable);
        AnsiConsole.WriteLine();

        // Record initial GC counts
        int initialGen0 = GC.CollectionCount(0);
        int initialGen1 = GC.CollectionCount(1);
        int initialGen2 = GC.CollectionCount(2);

        for (int iteration = 1; iteration <= iterations; iteration++)
        {
            AnsiConsole.Write(new Rule($"[bold yellow]Iteration {iteration} of {iterations}[/]").RuleStyle("yellow"));
            AnsiConsole.WriteLine();

            var hold = new List<byte[]>();
            long total = 0;

            try
            {
                int objectCount = 0;
                
                AnsiConsole.Status()
                    .Spinner(Spinner.Known.Dots)
                    .Start("[yellow]Allocating memory...[/]", ctx =>
                    {
                        while (total < bytesToAllocate)
                        {
                            long remaining = bytesToAllocate - total;
                            int thisChunk = (int)Math.Min(objectSize, remaining);

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
                            objectCount++;
                            
                            ctx.Status($"[yellow]Allocated {objectCount} objects ({FormatBytes(total)})[/]");
                        }
                    });

                AnsiConsole.MarkupLine($"[green]✓[/] Allocated [cyan]{objectCount}[/] objects totaling [cyan]{FormatBytes(total)}[/]");

                // Show memory statistics using .NET built-in APIs
                PrintMemoryStats();

                // Show GC activity since start
                PrintGCActivity(initialGen0, initialGen1, initialGen2);

                if (iteration < iterations)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[grey]Releasing memory (clearing references)...[/]");
                    
                    // Release all references - GC will collect when it decides to
                    hold.Clear();
                    
                    AnsiConsole.MarkupLine("[dim]Memory released. Waiting for GC to collect naturally...[/]");
                    AnsiConsole.MarkupLine("[grey]Press Enter to continue to next iteration...[/]");
                    Console.ReadLine();
                    
                    // Show GC activity after release
                    AnsiConsole.MarkupLine("\n[dim]GC activity after release:[/]");
                    PrintGCActivity(initialGen0, initialGen1, initialGen2);
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[grey]Press Enter to exit...[/]");
                    Console.ReadLine();
                }
            }
            catch (OutOfMemoryException)
            {
                AnsiConsole.MarkupLine($"[red]OutOfMemory[/] at ~{FormatBytes(total)} (observe RES).");
                Console.ReadLine();
                break;
            }
        }

        // Final summary
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule("[bold green]Final Summary[/]").RuleStyle("green"));
        AnsiConsole.WriteLine();
        PrintGCActivity(initialGen0, initialGen1, initialGen2);
    }

    /// <summary>
    /// Prints GC collection activity since the recorded initial counts.
    /// </summary>
    static void PrintGCActivity(int initialGen0, int initialGen1, int initialGen2)
    {
        int gen0Delta = GC.CollectionCount(0) - initialGen0;
        int gen1Delta = GC.CollectionCount(1) - initialGen1;
        int gen2Delta = GC.CollectionCount(2) - initialGen2;

        var gcTable = new Table()
            .Border(TableBorder.Simple)
            .BorderColor(Color.Grey)
            .AddColumn("[grey]Generation[/]")
            .AddColumn(new TableColumn("[grey]Collections[/]").RightAligned())
            .AddColumn(new TableColumn("[grey]Total[/]").RightAligned());

        gcTable.AddRow("Gen 0", $"[green]+{gen0Delta}[/]", $"[dim]{GC.CollectionCount(0)}[/]");
        gcTable.AddRow("Gen 1", $"[yellow]+{gen1Delta}[/]", $"[dim]{GC.CollectionCount(1)}[/]");
        gcTable.AddRow("Gen 2 (LOH)", $"[red]+{gen2Delta}[/]", $"[dim]{GC.CollectionCount(2)}[/]");

        AnsiConsole.Write(gcTable);
    }

    /// <summary>
    /// Parses command line arguments.
    /// </summary>
    static bool TryParseArguments(string[] args, out long bytesToAllocate, out int objectSize, out int iterations)
    {
        bytesToAllocate = 0;
        objectSize = 100 * 1024 * 1024; // Default: 100 MB (LOH)
        iterations = 1; // Default: single run

        string? sizeArg = null;

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] is "--objectSize" or "-o")
            {
                if (i + 1 >= args.Length)
                {
                    AnsiConsole.MarkupLine("[red]Error:[/] --objectSize requires a value.");
                    return false;
                }
                
                if (!TryParseSize(args[i + 1], out long objSizeBytes))
                {
                    AnsiConsole.MarkupLine($"[red]Error:[/] Invalid object size: {args[i + 1]}");
                    AnsiConsole.WriteLine();
                    PrintUsage();
                    return false;
                }
                
                if (objSizeBytes < 1 || objSizeBytes > int.MaxValue)
                {
                    AnsiConsole.MarkupLine($"[red]Error:[/] Object size must be between 1 byte and 2 GB.");
                    return false;
                }
                
                objectSize = (int)objSizeBytes;
                i++; // Skip the value
            }
            else if (args[i] is "--iterate" or "-i")
            {
                if (i + 1 >= args.Length)
                {
                    AnsiConsole.MarkupLine("[red]Error:[/] --iterate requires a value.");
                    return false;
                }
                
                if (!int.TryParse(args[i + 1], out iterations) || iterations < 1)
                {
                    AnsiConsole.MarkupLine($"[red]Error:[/] Invalid iteration count: {args[i + 1]}");
                    return false;
                }
                
                i++; // Skip the value
            }
            else if (!args[i].StartsWith("-"))
            {
                sizeArg = args[i];
            }
        }

        if (sizeArg == null)
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Please specify total size to allocate.");
            AnsiConsole.WriteLine();
            PrintUsage();
            return false;
        }

        if (!TryParseSize(sizeArg, out bytesToAllocate))
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Invalid size format.");
            AnsiConsole.WriteLine();
            PrintUsage();
            return false;
        }

        if (bytesToAllocate < 1024)
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Total size must be at least 1 KB.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Prints usage information with Spectre.Console formatting.
    /// </summary>
    static void PrintUsage()
    {
        // Title Panel
        var titlePanel = new Panel(
            new Markup("[bold]Allocates memory to demonstrate LOH vs SOH behavior.[/]\n\n" +
                       "Use [cyan]--objectSize[/] to control whether objects land in LOH or SOH.\n" +
                       "Each page is touched to ensure the OS actually commits the memory."))
            .Header("[bold blue]LOH Memory Reservation Tool[/]")
            .Border(BoxBorder.Rounded)
            .BorderColor(Color.Blue)
            .Padding(1, 0);

        AnsiConsole.Write(titlePanel);
        AnsiConsole.WriteLine();

        // Usage
        AnsiConsole.MarkupLine("[yellow]USAGE:[/]");
        AnsiConsole.MarkupLine("    ReserveMemory [grey]<size>[/] [grey dim][[options]][/]");
        AnsiConsole.MarkupLine("    ReserveMemory [grey]--help[/]");
        AnsiConsole.WriteLine();

        // Options Table
        var optionsTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .Title("[yellow]OPTIONS[/]")
            .AddColumn(new TableColumn("[grey]Option[/]").LeftAligned())
            .AddColumn(new TableColumn("[grey]Description[/]").LeftAligned())
            .AddColumn(new TableColumn("[grey]Default[/]").LeftAligned());

        optionsTable.AddRow("[green]<size>[/]", "Total memory to allocate per iteration", "[grey]-[/]");
        optionsTable.AddRow("[green]--objectSize, -o[/]", "Size of each allocated object", "[grey]100MB[/]");
        optionsTable.AddRow("[green]--iterate, -i[/]", "Number of allocation/release cycles", "[grey]1[/]");
        optionsTable.AddRow("[green]--help, -h[/]", "Show this help", "[grey]-[/]");

        AnsiConsole.Write(optionsTable);
        AnsiConsole.WriteLine();

        // Size Suffixes Table
        var suffixTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .Title("[yellow]SIZE SUFFIXES[/]")
            .AddColumn(new TableColumn("[grey]Suffix[/]").Centered())
            .AddColumn(new TableColumn("[grey]Unit[/]").LeftAligned())
            .AddColumn(new TableColumn("[grey]Bytes[/]").RightAligned());

        suffixTable.AddRow("[green]KB[/]", "Kilobytes", "1,024");
        suffixTable.AddRow("[green]MB[/]", "Megabytes", "1,048,576");
        suffixTable.AddRow("[green]GB[/]", "Gigabytes", "1,073,741,824");
        suffixTable.AddRow("[green]TB[/]", "Terabytes", "1,099,511,627,776");

        AnsiConsole.Write(suffixTable);
        AnsiConsole.WriteLine();

        // Examples
        AnsiConsole.MarkupLine("[yellow]EXAMPLES:[/]");
        var exampleTable = new Table()
            .Border(TableBorder.None)
            .HideHeaders()
            .AddColumn("Command")
            .AddColumn("Description");

        exampleTable.AddRow("[cyan]ReserveMemory 500MB[/]", "[grey]Allocate 500 MB in 100 MB objects (LOH)[/]");
        exampleTable.AddRow("[cyan]ReserveMemory 500MB --objectSize 100KB[/]", "[grey]Allocate 500 MB in 100 KB objects (LOH)[/]");
        exampleTable.AddRow("[cyan]ReserveMemory 500MB --objectSize 80KB[/]", "[grey]Allocate 500 MB in 80 KB objects (SOH)[/]");
        exampleTable.AddRow("[cyan]ReserveMemory 100MB -o 1MB[/]", "[grey]Allocate 100 MB in 1 MB objects (LOH)[/]");
        exampleTable.AddRow("[cyan]ReserveMemory 200MB -i 5[/]", "[grey]5 iterations of 200 MB allocation (observe GC)[/]");
        exampleTable.AddRow("[cyan]ReserveMemory 100MB -o 80KB -i 3[/]", "[grey]3 iterations with SOH objects[/]");

        AnsiConsole.Write(exampleTable);
        AnsiConsole.WriteLine();

        // LOH Info Panel
        var lohInfo = new Panel(
            new Markup($"[dim]LOH Threshold: [/][yellow]85,000 bytes (~85 KB)[/]\n\n" +
                       "[dim]Objects >= threshold → [/][red]LOH[/][dim] (Large Object Heap)[/]\n" +
                       "[dim]Objects <  threshold → [/][green]SOH[/][dim] (Small Object Heap)[/]\n\n" +
                       "[dim]LOH is only collected during Gen 2 garbage collections.\n" +
                       "Use --iterate to observe GC behavior across multiple cycles.[/]"))
            .Header("[bold grey]LOH INFO[/]")
            .Border(BoxBorder.Rounded)
            .BorderColor(Color.Grey)
            .Padding(1, 0);

        AnsiConsole.Write(lohInfo);
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
            AnsiConsole.MarkupLine("[bold yellow]Heap Size Distribution[/]");
            AnsiConsole.WriteLine();

            string[] barLabels = { "Gen 0", "Gen 1", "Gen 2", "LOH", "POH" };
            string[] barColors = { "green", "lime", "yellow", "red", "blue" };
            const int maxBarWidth = 40;

            for (int i = 0; i < genInfo.Length && i < barLabels.Length; i++)
            {
                long sizeBytes = genInfo[i].SizeAfterBytes;
                
                // Calculate bar width proportional to max size
                int barWidth = maxSize > 0 
                    ? (int)Math.Round((double)sizeBytes / maxSize * maxBarWidth) 
                    : 0;
                
                // Ensure at least 1 char if there's any data
                if (sizeBytes > 0 && barWidth == 0) barWidth = 1;
                
                string bar = new string('█', barWidth);
                string label = barLabels[i].PadRight(6);
                string formattedSize = FormatBytes(sizeBytes);
                
                AnsiConsole.MarkupLine($"  {label} [{barColors[i]}]{bar}[/] {formattedSize}");
            }
            
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

