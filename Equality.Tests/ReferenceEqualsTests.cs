namespace Equality.Tests;

/// <summary>
/// Tests for ReferenceEquals() method behavior
/// </summary>
public class ReferenceEqualsTests
{
    [Test]
    public void ReferenceTypes_ShouldCompareObjectReferences()
    {
        // Arrange
        var person1 = new Person("Alice", 30);
        var person2 = new Person("Alice", 30);
        var person3 = person1;

        // Act & Assert
        Assert.That(ReferenceEquals(person1, person2), Is.False, "Different object instances should have different references");
        Assert.That(ReferenceEquals(person1, person3), Is.True, "Same reference should return true");
    }

    [Test]
    public void BoxedValueTypes_ShouldCreateSeparateObjects()
    {
        // Arrange
        int value1 = 42;
        int value2 = 42;
        object boxed1 = value1;
        object boxed2 = value2;
        object boxed3 = value1;

        // Act & Assert
        Assert.That(ReferenceEquals(boxed1, boxed2), Is.False, "Boxing creates separate objects even with equal values");
        Assert.That(ReferenceEquals(boxed1, boxed3), Is.False, "Each boxing operation creates a new object");
    }

    [Test]
    public void BoxedStructs_ShouldCreateSeparateObjects()
    {
        // Arrange
        var point1 = new Point(1, 2);
        var point2 = new Point(1, 2);
        object boxedPoint1 = point1;
        object boxedPoint2 = point2;
        object boxedPoint3 = point1;

        // Act & Assert
        Assert.That(ReferenceEquals(boxedPoint1, boxedPoint2), Is.False, "Each struct boxing creates a new object");
        Assert.That(ReferenceEquals(boxedPoint1, boxedPoint3), Is.False, "Each struct boxing creates a new object, even for the same struct");
    }

    [Test]
    public void ReferenceEquals_WithNull_ShouldReturnTrue()
    {
        // Arrange
        Person? person = null;

        // Act & Assert
        Assert.That(ReferenceEquals(null, null), Is.True, "ReferenceEquals with both null should return true");
        Assert.That(ReferenceEquals(person, null), Is.True, "ReferenceEquals with one null should return true");
        Assert.That(ReferenceEquals(null, person), Is.True, "ReferenceEquals with one null should return true");
    }

    [Test]
    public void ReferenceEquals_WithSameObject_ShouldReturnTrue()
    {
        // Arrange
        var person = new Person("Alice", 30);

        // Act & Assert
        Assert.That(ReferenceEquals(person, person), Is.True, "ReferenceEquals with same object should return true");
    }

    [Test]
    public void ReferenceEquals_WithStringInterning_ShouldReturnTrueForSameString()
    {
        // Arrange
        string str1 = "Hello";
        string str2 = "Hello";
        string str3 = new string("Hello".ToCharArray());

        // Act & Assert
        // Note: String interning behavior may vary, but typically identical string literals share the same reference
        Assert.That(ReferenceEquals(str1, str2), Is.True, "Identical string literals should share the same reference due to interning");
        Assert.That(ReferenceEquals(str1, str3), Is.False, "Newly created strings should not share reference with interned strings");
    }
}