namespace _02_OpenForExtensionClosedForModification.GoodExample;

/// <summary>
/// ✅ Rectangle implements IShape - encapsulates its own area calculation.
/// </summary>
public class Rectangle : IShape
{
    public double Width { get; init; }
    public double Height { get; init; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public double CalculateArea() => Width * Height;
}

