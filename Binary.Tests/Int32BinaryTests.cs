namespace Binary.Tests;

/// <summary>
/// Tests demonstrating how int32 values are represented in binary format
/// </summary>
public class Int32BinaryRepresentationTests
{
    [Test]
    public void Int32BinaryRepresentation_PowerOfTwoValues_ShouldShowCorrectBinaryFormat()
    {
        // Test each power of 2 with individual assertions
        Assert.That(FormatInt32AsBinary(1  ), Is.EqualTo("0000 0000 0000 0000 0000 0000 0000 0001"), "Value 1 binary representation");
        Assert.That(FormatInt32AsBinary(2  ), Is.EqualTo("0000 0000 0000 0000 0000 0000 0000 0010"), "Value 2 binary representation");
        Assert.That(FormatInt32AsBinary(4  ), Is.EqualTo("0000 0000 0000 0000 0000 0000 0000 0100"), "Value 4 binary representation");
        Assert.That(FormatInt32AsBinary(8  ), Is.EqualTo("0000 0000 0000 0000 0000 0000 0000 1000"), "Value 8 binary representation");
        Assert.That(FormatInt32AsBinary(16 ), Is.EqualTo("0000 0000 0000 0000 0000 0000 0001 0000"), "Value 16 binary representation");
        Assert.That(FormatInt32AsBinary(32 ), Is.EqualTo("0000 0000 0000 0000 0000 0000 0010 0000"), "Value 32 binary representation");
        Assert.That(FormatInt32AsBinary(64 ), Is.EqualTo("0000 0000 0000 0000 0000 0000 0100 0000"), "Value 64 binary representation");
        Assert.That(FormatInt32AsBinary(128), Is.EqualTo("0000 0000 0000 0000 0000 0000 1000 0000"), "Value 128 binary representation");
        Assert.That(FormatInt32AsBinary(256), Is.EqualTo("0000 0000 0000 0000 0000 0001 0000 0000"), "Value 256 binary representation");
        Assert.That(FormatInt32AsBinary(512), Is.EqualTo("0000 0000 0000 0000 0000 0010 0000 0000"), "Value 512 binary representation");
    }

    [Test]
    public void Int32BinaryRepresentation_Zero_ShouldShowAllZeros()
    {
        Assert.That(FormatInt32AsBinary(0), Is.EqualTo("0000 0000 0000 0000 0000 0000 0000 0000"), "Zero should be represented as all zeros");
    }

    [Test]
    public void Int32BinaryRepresentation_MaxValue_ShouldShowCorrectBinary()
    {
        Assert.That(FormatInt32AsBinary(int.MaxValue), Is.EqualTo("0111 1111 1111 1111 1111 1111 1111 1111"), "int.MaxValue should have MSB as 0 and all other bits as 1");
    }

    [Test]
    public void Int32BinaryRepresentation_MinValue_ShouldShowCorrectBinary()
    {
        Assert.That(FormatInt32AsBinary(int.MinValue), Is.EqualTo("1000 0000 0000 0000 0000 0000 0000 0000"), "int.MinValue should have MSB as 1 and all other bits as 0");
    }

    [Test]
    public void Int32BinaryRepresentation_NegativeOne_ShouldShowTwosComplement()
    {
        Assert.That(FormatInt32AsBinary(-1), Is.EqualTo("1111 1111 1111 1111 1111 1111 1111 1111"), "-1 should be represented using two's complement (all bits set)");
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