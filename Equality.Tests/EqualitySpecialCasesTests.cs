namespace Equality.Tests;

/// <summary>
/// Tests demonstrating performance characteristics and best practices for equality comparisons
/// </summary>
public class EqualitySpecialCasesTests
{
    [Test]
    public void StructEquals_Vs_RecordStructEquals_Performance()
    {
        // Arrange
        var structPoint1 = new Point(1, 2);
        var structPoint2 = new Point(1, 2);
        var recordPoint1 = new RecordPoint(1, 2);
        var recordPoint2 = new RecordPoint(1, 2);

        // Act & Assert
        // Both should return true, but record struct should be faster due to compiler-generated implementation
        Assert.That(structPoint1.Equals(structPoint2), Is.True, "Struct Equals should work but may be slower");
        Assert.That(recordPoint1.Equals(recordPoint2), Is.True, "Record struct Equals should work and be faster");
    }

    [Test]
    public void ClassEquals_WithoutOverride_ShouldUseReferenceComparison()
    {
        // Arrange
        var person1 = new Person("Alice", 30);
        var person2 = new Person("Alice", 30);

        // Act & Assert
        // This demonstrates why you need to override Equals for value-based comparison
        Assert.That(person1.Equals(person2), Is.False, "Class without Equals override uses reference comparison");
        Assert.That(person1 == person2, Is.False, "Class without == override uses reference comparison");
    }

    [Test]
    public void RecordEquals_ShouldUseValueComparison()
    {
        // Arrange
        var record1 = new PersonRecord("Alice", 30);
        var record2 = new PersonRecord("Alice", 30);

        // Act & Assert
        // Records automatically implement value-based equality
        Assert.That(record1.Equals(record2), Is.True, "Record should use value comparison");
        Assert.That(record1 == record2, Is.True, "Record should use value comparison with == operator");
    }

    [Test]
    public void StringEquality_ShouldUseValueComparison()
    {
        // Arrange
        string str1 = "Hello";
        string str2 = new string("Hello".ToCharArray());
        string str3 = "Hello";

        // Act & Assert
        // String equality compares values, not references
        Assert.That(str1.Equals(str2), Is.True, "String.Equals should compare values");
        Assert.That(str1 == str2, Is.True, "String == should compare values");
        Assert.That(ReferenceEquals(str1, str3), Is.True, "String literals should share reference due to interning");
        Assert.That(ReferenceEquals(str1, str2), Is.False, "Newly created strings should not share reference");
    }

    [Test]
    public void NullHandling_InEqualityComparisons()
    {
        // Arrange
        Person? nullPerson = null;
        Person actualPerson = new Person("Alice", 30);

        // Act & Assert
        // Test null handling in different equality scenarios
        Assert.That(nullPerson == null, Is.True, "Null should equal null");
        Assert.That(actualPerson != null, Is.True, "Non-null should not equal null");
        
        // These would throw NullReferenceException:
        // Assert.That(nullPerson.Equals(actualPerson), Is.False); // Throws!
        // Assert.That(actualPerson.Equals(nullPerson), Is.False); // This works
        Assert.That(actualPerson.Equals(nullPerson), Is.False, "Equals with null should return false");
    }
}