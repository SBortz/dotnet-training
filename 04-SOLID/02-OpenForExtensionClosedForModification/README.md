# Open/Closed Principle (OCP)

> *"Software entities (classes, modules, functions, etc.) should be open for extension, but closed for modification."*

**Bertrand Meyer** introduced this principle in 1988. The idea is:

- **Open for extension**: You can add new behavior
- **Closed for modification**: You don't change existing code

---

## Summary

**Extend behavior by adding new code, not by changing existing code.**

When you have to modify existing classes to add new functionality:
- You risk breaking existing functionality
- You need to retest everything
- You create tight coupling to specific implementations

---

## The Shape Example

### ❌ Bad Example: Switch/If-Else Chain

```csharp
public class AreaCalculator
{
    public double CalculateArea(object shape)
    {
        if (shape is Rectangle r)
            return r.Width * r.Height;
        else if (shape is Circle c)
            return Math.PI * c.Radius * c.Radius;
        // Adding Triangle? Must MODIFY this class!
        throw new NotSupportedException();
    }
}
```

**Problems:**
- Adding a new shape (Triangle, Polygon) requires **modifying** `AreaCalculator`
- Every modification risks breaking existing calculations
- The class grows with every new shape type
- Violates the principle: class is **not closed** for modification

### ✅ Good Example: Polymorphism via Interface

```csharp
public interface IShape
{
    double CalculateArea();
}

public class Rectangle : IShape
{
    public double Width { get; init; }
    public double Height { get; init; }
    public double CalculateArea() => Width * Height;
}

public class Circle : IShape
{
    public double Radius { get; init; }
    public double CalculateArea() => Math.PI * Radius * Radius;
}

public class AreaCalculator
{
    public double CalculateTotalArea(IEnumerable<IShape> shapes) 
        => shapes.Sum(s => s.CalculateArea());
}
```

**Benefits:**
- Adding a new shape = **adding a new class** (extension)
- `AreaCalculator` **never changes** (closed for modification)
- Each shape is responsible for its own area calculation
- Easy to test each shape independently

---

## How to Achieve OCP

| Technique | Description |
|-----------|-------------|
| **Interfaces** | Define contracts that new implementations can fulfill |
| **Abstract Classes** | Provide base behavior with virtual/abstract methods |
| **Strategy Pattern** | Inject different algorithms at runtime |
| **Decorator Pattern** | Wrap objects to add behavior without modification |

---

## Common OCP Violations

| Violation | Smell | Solution |
|-----------|-------|----------|
| Switch on type | `switch (shape.Type)` | Polymorphism |
| If-else chains | `if (x is A) ... else if (x is B)` | Interface + implementations |
| Enum-based logic | `switch (status) { case Status.New: ... }` | State pattern or strategy |
| Hard-coded dependencies | `new ConcreteService()` | Dependency injection |

---

## Key Insight

The goal is to design systems where **new requirements = new classes**, not modified classes.

```
❌ New feature → Modify existing class → Risk regression → Retest everything
✅ New feature → Add new class → Existing code untouched → Only test new code
```

---

