namespace _02_OpenClosedPrinciple.BadExample;

public class AreaCalculator
{
    public double CalculateArea(object shape)
    {
        if (shape is Rectangle rectangle)
            return rectangle.Width * rectangle.Height;
        
        if (shape is Circle circle)
            return Math.PI * circle.Radius * circle.Radius;
        
        // Adding Triangle? You must modify this class!
        throw new NotSupportedException($"Shape type '{shape.GetType().Name}' is not supported.");
    }

    public double CalculateTotalArea(IEnumerable<object> shapes)
    {
        return shapes.Sum(shape => CalculateArea(shape));
    }
}

public record Rectangle(double Width, double Height);

public record Circle(double Radius);
