namespace _02_OpenClosedPrinciple;

using Bad = _02_OpenClosedPrinciple.BadExample;
using Good = _02_OpenClosedPrinciple.GoodExample;

public class OpenClosedTests
{
    [Test]
    public void BadExample_AreaCalculator_MustBeModifiedForNewShapes()
    {
        var calculator = new Bad.AreaCalculator();
        
        Assert.That(calculator.CalculateArea(new Bad.Rectangle(5, 4)), Is.EqualTo(20.0));
        Assert.That(calculator.CalculateArea(new Bad.Circle(3)), Is.EqualTo(Math.PI * 9).Within(0.0001));
        
        // Any new shape fails - AreaCalculator must be modified to support it
        var unknownShape = new { SomeProperty = 42 };
        Assert.Throws<NotSupportedException>(() => calculator.CalculateArea(unknownShape));
    }

    [Test]
    public void GoodExample_NewShapesCanBeAdded_WithoutModifyingAreaCalculator()
    {
        var calculator = new Good.AreaCalculator();
        
        var shapes = new Good.IShape[]
        {
            new Good.Rectangle(4, 5),
            new Good.Circle(2),
            new Good.Triangle(6, 4),
            new Hexagon(3) // Custom shape works immediately
        };

        var totalArea = calculator.CalculateTotalArea(shapes);
        
        var expected = 20 + (Math.PI * 4) + 12 + ((3 * Math.Sqrt(3) / 2) * 9);
        Assert.That(totalArea, Is.EqualTo(expected).Within(0.0001));
    }

    private class Hexagon(double sideLength) : Good.IShape
    {
        public double CalculateArea() => (3 * Math.Sqrt(3) / 2) * sideLength * sideLength;
    }
}
