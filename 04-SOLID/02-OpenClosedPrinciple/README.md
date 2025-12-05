# Open/Closed Principle (OCP)

> *"Software entities should be open for extension, but closed for modification."*
> — Bertrand Meyer, 1988

- **Open for extension**: You can add new behavior
- **Closed for modification**: You don't change existing code

---

## The Shape Example

### Bad: Switch/If-Else Chain

```csharp
public class AreaCalculator
{
    public double CalculateArea(object shape)
    {
        if (shape is Rectangle r)
            return r.Width * r.Height;
        if (shape is Circle c)
            return Math.PI * c.Radius * c.Radius;
        // Adding Triangle? This class must be modified!
        throw new NotSupportedException();
    }
}
```

Each new shape requires modifying `AreaCalculator`.

### Good: Polymorphism via Interface

```csharp
public interface IShape
{
    double CalculateArea();
}

public class Rectangle(double width, double height) : IShape
{
    public double CalculateArea() => width * height;
}

public class AreaCalculator
{
    public double CalculateTotalArea(IEnumerable<IShape> shapes) 
        => shapes.Sum(s => s.CalculateArea());
}
```

New shapes = new classes. `AreaCalculator` stays unchanged.

---

## Techniques for OCP

| Technique | Description |
|-----------|-------------|
| Interfaces | Contracts that new implementations can fulfill |
| Abstract Classes | Base behavior with virtual/abstract methods |
| Strategy Pattern | Inject different algorithms at runtime |

---

## Typical Violations

| Code Smell | Solution |
|------------|----------|
| `switch (shape.Type)` | Polymorphism |
| `if (x is A) ... else if (x is B)` | Interface + implementations |
| `switch (status) { case New: ... }` | State/Strategy Pattern |

---
