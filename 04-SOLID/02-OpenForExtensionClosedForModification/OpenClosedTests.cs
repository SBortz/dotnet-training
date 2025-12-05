namespace _02_OpenForExtensionClosedForModification;

using Bad = _02_OpenForExtensionClosedForModification.BadExample;
using Good = _02_OpenForExtensionClosedForModification.GoodExample;

public class OpenClosedTests
{
    [Test]
    public void BadExample_AreaCalculator_MustBeModifiedForNewShapes()
    {
        // ❌ Problem: Adding a new shape type throws until AreaCalculator is MODIFIED
        var calculator = new Bad.AreaCalculator();
        
        // These work because they're hardcoded in the if-else chain
        Assert.That(calculator.CalculateArea(new Bad.Rectangle(5, 4)), Is.EqualTo(20.0));
        Assert.That(calculator.CalculateArea(new Bad.Circle(3)), Is.EqualTo(Math.PI * 9).Within(0.0001));
        
        // But any new shape fails - AreaCalculator must be MODIFIED to support it
        var unknownShape = new { SomeProperty = 42 };
        Assert.Throws<NotSupportedException>(() => calculator.CalculateArea(unknownShape));
    }

    [Test]
    public void GoodExample_NewShapesCanBeAdded_WithoutModifyingAreaCalculator()
    {
        // ✅ AreaCalculator works with ANY IShape - no modifications needed
        var calculator = new Good.AreaCalculator();
        
        var shapes = new Good.IShape[]
        {
            new Good.Rectangle(4, 5),   // 20
            new Good.Circle(2),         // π * 4 ≈ 12.566
            new Good.Triangle(6, 4),    // 12
            new Hexagon(3)              // Custom shape - works immediately!
        };

        var totalArea = calculator.CalculateTotalArea(shapes);
        
        var expectedArea = 20 + (Math.PI * 4) + 12 + ((3 * Math.Sqrt(3) / 2) * 9);
        Assert.That(totalArea, Is.EqualTo(expectedArea).Within(0.0001));
    }

    /// <summary>
    /// A new shape added to demonstrate OCP.
    /// Notice: AreaCalculator was NOT modified to support this!
    /// </summary>
    private class Hexagon : Good.IShape
    {
        private readonly double _sideLength;

        public Hexagon(double sideLength) => _sideLength = sideLength;

        public double CalculateArea() => (3 * Math.Sqrt(3) / 2) * _sideLength * _sideLength;
    }
}
