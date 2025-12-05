namespace _02_OpenClosedPrinciple.GoodExample;

public class Triangle(double @base, double height) : IShape
{
    public double Base { get; } = @base;
    public double Height { get; } = height;

    public double CalculateArea() => 0.5 * Base * Height;
}
