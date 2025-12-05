namespace _02_OpenClosedPrinciple.GoodExample;

public class AreaCalculator
{
    public double CalculateArea(IShape shape) => shape.CalculateArea();

    public double CalculateTotalArea(IEnumerable<IShape> shapes) => shapes.Sum(s => s.CalculateArea());
}
