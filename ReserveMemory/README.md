# ReserveMemory - LOH Memory Allocation Tool

A .NET console application to demonstrate and explore **Large Object Heap (LOH)** vs **Small Object Heap (SOH)** behavior in the .NET runtime.

## Purpose

This tool helps you understand how .NET's garbage collector handles memory allocation differently based on object size:

- **LOH (Large Object Heap)**: Objects ≥ 85,000 bytes (~85 KB)
- **SOH (Small Object Heap)**: Objects < 85,000 bytes

Key LOH characteristics:
- LOH is only collected during **Gen 2** (full) garbage collections
- LOH is **not compacted** by default (can lead to fragmentation)
- LOH objects are immediately placed in Gen 2

## Installation

```bash
cd ReserveMemory
dotnet restore
dotnet build
```

## Usage

```bash
ReserveMemory <size> [options]
```

### Parameters

| Option | Short | Description | Default |
|--------|-------|-------------|---------|
| `<size>` | - | Total memory to allocate per iteration | (required) |
| `--objectSize` | `-o` | Size of each allocated object | 100MB |
| `--iterate` | `-i` | Number of allocation/release cycles | 1 |
| `--help` | `-h` | Show help | - |

### Size Suffixes

| Suffix | Unit | Bytes |
|--------|------|-------|
| KB | Kilobytes | 1,024 |
| MB | Megabytes | 1,048,576 |
| GB | Gigabytes | 1,073,741,824 |
| TB | Terabytes | 1,099,511,627,776 |

Decimal values are supported (e.g., `1.5GB`).

## Examples

### Basic Allocation (LOH)

```bash
# Allocate 500 MB in 100 MB objects (default, goes to LOH)
dotnet run -- 500MB
```

### Control Object Size

```bash
# Allocate 500 MB in 100 KB objects (LOH, >= 85 KB threshold)
dotnet run -- 500MB --objectSize 100KB

# Allocate 500 MB in 80 KB objects (SOH, < 85 KB threshold)
dotnet run -- 500MB --objectSize 80KB

# Short form
dotnet run -- 500MB -o 1MB
```

### Multiple Iterations (Observe GC)

```bash
# 5 iterations - observe GC behavior over multiple cycles
dotnet run -- 200MB -i 5

# Compare LOH vs SOH GC behavior
dotnet run -- 100MB -o 100KB -i 3   # LOH objects
dotnet run -- 100MB -o 80KB -i 3    # SOH objects
```

## Output

The tool displays:

1. **Configuration Summary**: Total size, object size, target heap (LOH/SOH)
2. **Memory Statistics**: Detailed .NET GC memory information
3. **Heap Generation Sizes**: Visual bar chart showing Gen 0/1/2, LOH, and POH sizes
4. **GC Collection Counts**: Number of collections per generation
5. **GC Settings**: Current LOH compaction mode, latency mode, server GC status

### Sample Output

```
───────────────────────── Memory Allocation ─────────────────────────

╭───────────────────┬────────────╮
│ Setting           │ Value      │
├───────────────────┼────────────┤
│ Total to allocate │ 500.00 MB  │
│ Object size       │ 100.00 MB  │
│ LOH threshold     │ 83.01 KB   │
│ Target heap       │ LOH        │
│ Iterations        │ 1          │
│ Page size         │ 4096 bytes │
╰───────────────────┴────────────╯

✓ Allocated 5 objects totaling 500.00 MB

Heap Size Distribution

  Gen 0  █ 54.67 KB
  Gen 1  █ 24.00 KB
  Gen 2   0.00 B
  LOH    ████████████████████████████████████████ 500.00 MB
  POH    █ 8.00 KB
```

## Technical Details

### Memory Touching

The tool "touches" every page of allocated memory using a pseudo-random pattern (xorshift32). This ensures:
- The OS actually commits the physical memory
- Memory isn't just reserved but actively used
- Realistic memory pressure for GC observation

### GC Settings

The tool explicitly sets LOH compaction mode to `Default` (no compaction) to demonstrate standard LOH behavior. In production, you can enable compaction with:

```csharp
GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
GC.Collect();
```

### No Forced GC

When using `--iterate`, the tool releases memory by clearing references but does **not** call `GC.Collect()`. This allows you to observe natural GC behavior.

## Requirements

- .NET 9.0 SDK
- [Spectre.Console](https://spectreconsole.net/) (automatically restored)

## License

MIT

