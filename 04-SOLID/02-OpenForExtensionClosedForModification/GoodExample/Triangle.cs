namespace _02_OpenForExtensionClosedForModification.GoodExample;

/// <summary>
/// ✅ Triangle was added WITHOUT modifying any existing code!
/// This demonstrates OCP: open for extension (new class), closed for modification.
/// </summary>
public class Triangle : IShape
{
    public double Base { get; init; }
    public double Height { get; init; }

    public Triangle(double @base, double height)
    {
        Base = @base;
        Height = height;
    }

    public double CalculateArea() => 0.5 * Base * Height;
}

