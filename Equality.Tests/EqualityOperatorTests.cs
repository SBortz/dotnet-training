using System.Drawing;

namespace Equality.Tests;

/// <summary>
/// Tests for the == operator behavior across different data types
/// </summary>
public class EqualityOperatorTests
{
    [Test]
    public void ValueTypes_Int_ShouldCompareValues()
    {
        // Arrange
        int a = 5;
        int b = 5;
        int c = 10;

        // Act & Assert
        Assert.That(a == b, Is.True, "Equal int values should be equal");
        Assert.That(a == c, Is.False, "Different int values should not be equal");
    }

    [Test]
    public void ValueTypes_RecordStruct_ShouldCompareValues()
    {
        // Arrange
        var rp1 = new RecordPoint(1, 2);
        var rp2 = new RecordPoint(1, 2);
        var rp3 = new RecordPoint(3, 4);

        // Act & Assert
        Assert.That(rp1 == rp2, Is.True, "Equal record struct values should be equal");
        Assert.That(rp1 == rp3, Is.False, "Different record struct values should not be equal");
    }

    [Test]
    public void ReferenceTypes_Class_ShouldCompareReferences()
    {
        // Arrange
        var person1 = new Person("Alice", 30);
        var person2 = new Person("Alice", 30);
        var person3 = person1;

        // Act & Assert
        Assert.That(person1 == person2, Is.False, "Different class instances should not be equal (reference comparison)");
        Assert.That(person1 == person3, Is.True, "Same reference should be equal");
    }

    [Test]
    public void ReferenceTypes_Record_ShouldCompareValues()
    {
        // Arrange
        var pr1 = new PersonRecord("Bob", 25);
        var pr2 = new PersonRecord("Bob", 25);
        var pr3 = new PersonRecord("Charlie", 35);

        // Act & Assert
        Assert.That(pr1 == pr2, Is.True, "Equal record values should be equal");
        Assert.That(pr1 == pr3, Is.False, "Different record values should not be equal");
    }

    [Test]
    public void ReferenceTypes_String_ShouldCompareValues()
    {
        // Arrange
        string str1 = "Hello";
        string str2 = "Hello";
        string str3 = "World";

        // Act & Assert
        Assert.That(str1 == str2, Is.True, "Equal string values should be equal");
        Assert.That(str1 == str3, Is.False, "Different string values should not be equal");
    }

    [Test]
    public void ValueTypes_Struct_ShouldNotCompileWithEqualityOperator()
    {
        // Arrange
        var point1 = new Point(1, 2);
        var point2 = new Point(1, 2);

        // Act & Assert
        // Note: This test documents the behavior that structs without explicit == operator
        // cannot use the == operator. In a real scenario, this would be a compilation error.
        // We test the Equals method instead, which is available.
        Assert.That(point1.Equals(point2), Is.True, "Structs should use Equals method, not == operator");
    }
}