# Open/Closed Principle (OCP)

> *"Software entities should be open for extension, but closed for modification."*
> — Bertrand Meyer, 1988

- **Open for extension**: Du kannst neues Verhalten hinzufügen
- **Closed for modification**: Du änderst keinen bestehenden Code

---

## Das Shape-Beispiel

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
        // Triangle hinzufügen? Diese Klasse muss geändert werden!
        throw new NotSupportedException();
    }
}
```

Jede neue Form erfordert eine Änderung an `AreaCalculator`.

### Good: Polymorphismus via Interface

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

Neue Forms = neue Klassen. `AreaCalculator` bleibt unverändert.

---

## Techniken für OCP

| Technik | Beschreibung |
|---------|--------------|
| Interfaces | Contracts die neue Implementierungen erfüllen können |
| Abstract Classes | Basisverhalten mit virtual/abstract Methods |
| Strategy Pattern | Verschiedene Algorithmen zur Laufzeit injizieren |

---

## Typische Verstöße

| Code Smell | Lösung |
|------------|--------|
| `switch (shape.Type)` | Polymorphismus |
| `if (x is A) ... else if (x is B)` | Interface + Implementierungen |
| `switch (status) { case New: ... }` | State/Strategy Pattern |

---
