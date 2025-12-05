namespace _02_OpenForExtensionClosedForModification.GoodExample;

/// <summary>
/// ✅ The key to OCP: Define a contract that allows extension without modification.
/// New shapes implement this interface - no existing code needs to change.
/// </summary>
public interface IShape
{
    /// <summary>
    /// Each shape knows how to calculate its own area.
    /// </summary>
    double CalculateArea();
}

