namespace Binary.Tests;

/// <summary>
/// Tests demonstrating how int32 values are represented in binary format
/// </summary>
public class Int32BinaryRepresentationTests
{
    [Test]
    public void Int32BinaryRepresentation_PowerOfTwoValues_ShouldShowCorrectBinaryFormat()
    {
        // Arrange - Test data for powers of 2
        var testCases = new[]
        {
            new { Value = 1, ExpectedBinary = "0000 0000 0000 0000 0000 0000 0000 0001" },
            new { Value = 2, ExpectedBinary = "0000 0000 0000 0000 0000 0000 0000 0010" },
            new { Value = 4, ExpectedBinary = "0000 0000 0000 0000 0000 0000 0000 0100" },
            new { Value = 8, ExpectedBinary = "0000 0000 0000 0000 0000 0000 0000 1000" },
            new { Value = 16, ExpectedBinary = "0000 0000 0000 0000 0000 0000 0001 0000" },
            new { Value = 32, ExpectedBinary = "0000 0000 0000 0000 0000 0000 0010 0000" },
            new { Value = 64, ExpectedBinary = "0000 0000 0000 0000 0000 0000 0100 0000" },
            new { Value = 128, ExpectedBinary = "0000 0000 0000 0000 0000 0000 1000 0000" },
            new { Value = 256, ExpectedBinary = "0000 0000 0000 0000 0000 0001 0000 0000" },
            new { Value = 512, ExpectedBinary = "0000 0000 0000 0000 0000 0010 0000 0000" }
        };

        // Act & Assert
        foreach (var testCase in testCases)
        {
            string actualBinary = FormatInt32AsBinary(testCase.Value);
            
            Assert.That(actualBinary, Is.EqualTo(testCase.ExpectedBinary), 
                $"Value {testCase.Value} should be represented as {testCase.ExpectedBinary}");
            
            // Additional verification: Convert back to ensure correctness
            int convertedBack = ConvertBinaryStringToInt32(actualBinary);
            Assert.That(convertedBack, Is.EqualTo(testCase.Value), 
                $"Binary conversion should be reversible for value {testCase.Value}");
        }
    }

    [Test]
    public void Int32BinaryRepresentation_Zero_ShouldShowAllZeros()
    {
        // Arrange
        int value = 0;
        string expectedBinary = "0000 0000 0000 0000 0000 0000 0000 0000";

        // Act
        string actualBinary = FormatInt32AsBinary(value);

        // Assert
        Assert.That(actualBinary, Is.EqualTo(expectedBinary), "Zero should be represented as all zeros");
    }

    [Test]
    public void Int32BinaryRepresentation_MaxValue_ShouldShowCorrectBinary()
    {
        // Arrange
        int value = int.MaxValue; // 2,147,483,647
        string expectedBinary = "0111 1111 1111 1111 1111 1111 1111 1111";

        // Act
        string actualBinary = FormatInt32AsBinary(value);

        // Assert
        Assert.That(actualBinary, Is.EqualTo(expectedBinary), "int.MaxValue should have MSB as 0 and all other bits as 1");
    }

    [Test]
    public void Int32BinaryRepresentation_MinValue_ShouldShowCorrectBinary()
    {
        // Arrange
        int value = int.MinValue; // -2,147,483,648
        string expectedBinary = "1000 0000 0000 0000 0000 0000 0000 0000";

        // Act
        string actualBinary = FormatInt32AsBinary(value);

        // Assert
        Assert.That(actualBinary, Is.EqualTo(expectedBinary), "int.MinValue should have MSB as 1 and all other bits as 0");
    }

    [Test]
    public void Int32BinaryRepresentation_NegativeOne_ShouldShowTwosComplement()
    {
        // Arrange
        int value = -1;
        string expectedBinary = "1111 1111 1111 1111 1111 1111 1111 1111";

        // Act
        string actualBinary = FormatInt32AsBinary(value);

        // Assert
        Assert.That(actualBinary, Is.EqualTo(expectedBinary), "-1 should be represented using two's complement (all bits set)");
    }

    /// <summary>
    /// Helper method to format an int32 as a binary string with proper spacing
    /// </summary>
    private static string FormatInt32AsBinary(int value)
    {
        // Convert to binary string (32 bits)
        string binary = Convert.ToString(value, 2).PadLeft(32, '0');
        
        // Add spaces every 4 bits for readability
        var result = new System.Text.StringBuilder();
        for (int i = 0; i < binary.Length; i += 4)
        {
            if (i > 0) result.Append(' ');
            result.Append(binary.Substring(i, 4));
        }
        
        return result.ToString();
    }

    /// <summary>
    /// Helper method to convert a formatted binary string back to int32
    /// </summary>
    private static int ConvertBinaryStringToInt32(string binaryString)
    {
        // Remove spaces and convert back to int
        string cleanBinary = binaryString.Replace(" ", "");
        return Convert.ToInt32(cleanBinary, 2);
    }
}