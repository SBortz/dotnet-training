namespace _02_OpenClosedPrinciple.GoodExample;

public class Circle(double radius) : IShape
{
    public double Radius { get; } = radius;

    public double CalculateArea() => Math.PI * Radius * Radius;
}
