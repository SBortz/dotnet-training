namespace _02_OpenClosedPrinciple.GoodExample;

public class Rectangle(double width, double height) : IShape
{
    public double Width { get; } = width;
    public double Height { get; } = height;

    public double CalculateArea() => Width * Height;
}
