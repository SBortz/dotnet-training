namespace _02_OpenForExtensionClosedForModification.BadExample;

/// <summary>
/// ❌ Violates OCP - Adding a new shape requires MODIFYING this class.
/// The class is NOT closed for modification.
/// </summary>
public class AreaCalculator
{
    /// <summary>
    /// Every new shape type requires adding another if-else branch here.
    /// This means the class must be modified (and retested) for every extension.
    /// </summary>
    public double CalculateArea(object shape)
    {
        if (shape is Rectangle rectangle)
        {
            return rectangle.Width * rectangle.Height;
        }
        else if (shape is Circle circle)
        {
            return Math.PI * circle.Radius * circle.Radius;
        }
        // ⚠️ Want to add Triangle? You must MODIFY this class!
        // else if (shape is Triangle triangle)
        // {
        //     return 0.5 * triangle.Base * triangle.Height;
        // }
        
        throw new NotSupportedException($"Shape type '{shape.GetType().Name}' is not supported.");
    }

    /// <summary>
    /// Same problem: if-else chain that must be modified for each new shape.
    /// </summary>
    public double CalculateTotalArea(IEnumerable<object> shapes)
    {
        double total = 0;
        foreach (var shape in shapes)
        {
            total += CalculateArea(shape);
        }
        return total;
    }
}

public record Rectangle(double Width, double Height);

public record Circle(double Radius);

// Adding this would require modifying AreaCalculator above!
// public record Triangle(double Base, double Height);

