# .NET Training

A collection of .NET projects for learning and demonstrating various C# and .NET runtime concepts.

## Projects

### 01-DataTypes

| Project | Description |
|---------|-------------|
| [Binary.Tests](./01-DataTypes/Binary.Tests/) | Tests exploring binary representation of numbers (`int`, `decimal`) |
| [Collection.Tests](./01-DataTypes/Collection.Tests/) | Tests exploring `List<T>` internals and collection behavior |
| [Sorting.Tests](./01-DataTypes/Sorting.Tests/) | Sorting algorithm implementations (BubbleSort, QuickSort) with performance tests |

### 02-Equality

| Project | Description |
|---------|-------------|
| [Equality.Lib](./02-Equality/Equality.Lib/) | Library demonstrating C# equality concepts (`==`, `.Equals()`, `ReferenceEquals()`) |
| [Equality](./02-Equality/Equality/) | Console app with equality comparison examples |
| [Equality.Tests](./02-Equality/Equality.Tests/) | Unit tests for equality behavior with different types |

### 03-Memory

| Project | Description |
|---------|-------------|
| [ReserveMemory](./03-Memory/ReserveMemory/) | Tool to demonstrate LOH vs SOH memory allocation and GC behavior |

### 04-SOLID

| Project | Prinzip | Description |
|---------|---------|-------------|
| [01-SingleResponsibilityPrinciple](./04-SOLID/01-SingleResponsibilityPrinciple/) | SRP | Eine Klasse, ein Änderungsgrund |
| [02-OpenClosedPrinciple](./04-SOLID/02-OpenClosedPrinciple/) | OCP | Offen für Erweiterung, geschlossen für Modifikation |
| [03-LiskovSubstitutionPattern.Tests](./04-SOLID/03-LiskovSubstitutionPattern.Tests/) | LSP | Subtypen müssen Basistypen ersetzen können |
| [04-InterfaceSegregationPrinciple](./04-SOLID/04-InterfaceSegregationPrinciple/) | ISP | Kleine, spezifische Interfaces statt großer |
| [05-DependencyInversionPrinciple](./04-SOLID/05-DependencyInversionPrinciple/) | DIP | Abhängig von Abstraktionen, nicht Implementierungen |

## Quick Start

```bash
# Clone and restore
git clone <repo-url>
cd dotnet-training
dotnet restore

# Run all tests
dotnet test

# Run specific project
dotnet run --project 03-Memory/ReserveMemory -- 500MB
dotnet run --project 02-Equality/Equality
```

## Project Details

### Equality

Demonstrates how equality works in C# across different types:
- `==` operator behavior (value vs reference comparison)
- `.Equals()` method behavior
- `ReferenceEquals()` for identity checks
- Differences between `class`, `struct`, `record`, and `record struct`

[Read more](./02-Equality/Equality.Lib/README.md)

---

### ReserveMemory

Interactive tool to explore .NET memory management:
- Allocate memory on LOH (Large Object Heap) or SOH (Small Object Heap)
- Observe GC behavior across multiple allocation cycles
- Visualize heap generation sizes

```bash
# Allocate 500MB in LOH
dotnet run --project 03-Memory/ReserveMemory -- 500MB

# Allocate in SOH (objects < 85KB)
dotnet run --project 03-Memory/ReserveMemory -- 500MB --objectSize 80KB

# Multiple iterations to observe GC
dotnet run --project 03-Memory/ReserveMemory -- 200MB -i 5
```

[Read more](./03-Memory/ReserveMemory/README.md)

---

### Sorting.Tests

Sorting algorithm implementations with correctness and performance tests:
- BubbleSort (O(n²))
- QuickSort (O(n log n))
- Performance benchmarks comparing both algorithms

---

### Binary.Tests

Explore binary representation of .NET numeric types:
- `int` (32-bit two's complement)
- `decimal` (128-bit with sign, scale, and mantissa)

---

### Collection.Tests

Explore `List<T>` internals:
- Capacity growth behavior
- Memory allocation patterns

## Requirements

- .NET 10.0 SDK

## License

MIT
