namespace _02_OpenForExtensionClosedForModification.GoodExample;

/// <summary>
/// ✅ Follows OCP - This class NEVER needs to change when new shapes are added.
/// It works with the IShape abstraction, not concrete types.
/// </summary>
public class AreaCalculator
{
    /// <summary>
    /// Works with any IShape - new shapes can be added without modifying this method.
    /// </summary>
    public double CalculateArea(IShape shape)
    {
        return shape.CalculateArea();
    }

    /// <summary>
    /// Calculates total area for any collection of shapes.
    /// Adding Triangle, Polygon, or any future shape requires ZERO changes here.
    /// </summary>
    public double CalculateTotalArea(IEnumerable<IShape> shapes)
    {
        return shapes.Sum(shape => shape.CalculateArea());
    }
}

