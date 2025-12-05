namespace _02_OpenForExtensionClosedForModification.GoodExample;

/// <summary>
/// ✅ Circle implements IShape - encapsulates its own area calculation.
/// </summary>
public class Circle : IShape
{
    public double Radius { get; init; }

    public Circle(double radius)
    {
        Radius = radius;
    }

    public double CalculateArea() => Math.PI * Radius * Radius;
}

