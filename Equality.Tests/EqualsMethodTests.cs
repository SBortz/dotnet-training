namespace Equality.Tests;

/// <summary>
/// Tests for the .Equals() method behavior across different data types
/// </summary>
public class EqualsMethodTests
{
    [Test]
    public void ValueTypes_Int_ShouldCompareValues()
    {
        // Arrange
        int a = 5;
        int b = 5;
        int c = 10;

        // Act & Assert
        Assert.That(a.Equals(b), Is.True, "Equal int values should be equal");
        Assert.That(a.Equals(c), Is.False, "Different int values should not be equal");
    }

    [Test]
    public void ValueTypes_Struct_ShouldCompareValuesUsingValueTypeEquals()
    {
        // Arrange
        var point1 = new Point(1, 2);
        var point2 = new Point(1, 2);
        var point3 = new Point(3, 4);

        // Act & Assert
        Assert.That(point1.Equals(point2), Is.True, "Equal struct values should be equal using ValueType.Equals");
        Assert.That(point1.Equals(point3), Is.False, "Different struct values should not be equal");
    }

    [Test]
    public void ValueTypes_RecordStruct_ShouldCompareValuesUsingCompilerGeneratedEquals()
    {
        // Arrange
        var rp1 = new RecordPoint(1, 2);
        var rp2 = new RecordPoint(1, 2);
        var rp3 = new RecordPoint(3, 4);

        // Act & Assert
        Assert.That(rp1.Equals(rp2), Is.True, "Equal record struct values should be equal using compiler generated Equals");
        Assert.That(rp1.Equals(rp3), Is.False, "Different record struct values should not be equal");
    }

    [Test]
    public void ReferenceTypes_Class_ShouldCompareReferencesByDefault()
    {
        // Arrange
        var person1 = new Person("Alice", 30);
        var person2 = new Person("Alice", 30);
        var person3 = person1;

        // Act & Assert
        Assert.That(person1.Equals(person2), Is.False, "Different class instances should not be equal (reference comparison by default)");
        Assert.That(person1.Equals(person3), Is.True, "Same reference should be equal");
    }

    [Test]
    public void ReferenceTypes_Record_ShouldCompareValuesUsingCompilerGeneratedEquals()
    {
        // Arrange
        var pr1 = new PersonRecord("Bob", 25);
        var pr2 = new PersonRecord("Bob", 25);
        var pr3 = new PersonRecord("Charlie", 35);

        // Act & Assert
        Assert.That(pr1.Equals(pr2), Is.True, "Equal record values should be equal using compiler generated Equals");
        Assert.That(pr1.Equals(pr3), Is.False, "Different record values should not be equal");
    }

    [Test]
    public void ReferenceTypes_String_ShouldCompareValuesDirectly()
    {
        // Arrange
        string str1 = "Hello";
        string str2 = "Hello";
        string str3 = "World";

        // Act & Assert
        Assert.That(str1.Equals(str2), Is.True, "Equal string values should be equal using direct value comparison");
        Assert.That(str1.Equals(str3), Is.False, "Different string values should not be equal");
    }

    [Test]
    public void EqualsMethod_WithNullReference_ShouldReturnFalse()
    {
        // Arrange
        var person = new Person("Alice", 30);

        // Act & Assert
        Assert.That(person.Equals(null), Is.False, "Equals with null should return false");
    }

    [Test]
    public void EqualsMethod_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var person = new Person("Alice", 30);
        var point = new Point(1, 2);

        // Act & Assert
        Assert.That(person.Equals(point), Is.False, "Equals with different type should return false");
    }
}