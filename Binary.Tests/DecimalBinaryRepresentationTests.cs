namespace Binary.Tests;

/// <summary>
/// Tests demonstrating how decimal values are represented in binary format
/// Note: Decimal is a 128-bit value with 96-bit mantissa and 32-bit scale/sign
/// </summary>
public class DecimalBinaryRepresentationTests
{
    [Test]
    public void DecimalBinaryRepresentation_PowerOfTenValues_ShouldShowCorrectBinaryFormat()
    {
        // Test each power of 10 with individual assertions
        Assert.That(FormatDecimalAsBinary(1m), Is.EqualTo("0000 0000 0000 0000 0000 0000 0000 0001 0000 0000 0000 0000 0000 0000 0000 0000"), "Value 1 decimal binary representation");
        Assert.That(FormatDecimalAsBinary(10m), Is.EqualTo("0000 0000 0000 0000 0000 0000 0000 000A 0000 0000 0000 0000 0000 0000 0000 0001"), "Value 10 decimal binary representation");
        Assert.That(FormatDecimalAsBinary(100m), Is.EqualTo("0000 0000 0000 0000 0000 0000 0000 0064 0000 0000 0000 0000 0000 0000 0000 0002"), "Value 100 decimal binary representation");
        Assert.That(FormatDecimalAsBinary(1000m), Is.EqualTo("0000 0000 0000 0000 0000 0000 0000 03E8 0000 0000 0000 0000 0000 0000 0000 0003"), "Value 1000 decimal binary representation");
        Assert.That(FormatDecimalAsBinary(10000m), Is.EqualTo("0000 0000 0000 0000 0000 0000 0000 2710 0000 0000 0000 0000 0000 0000 0000 0004"), "Value 10000 decimal binary representation");
        Assert.That(FormatDecimalAsBinary(100000m), Is.EqualTo("0000 0000 0000 0000 0000 0000 0001 86A0 0000 0000 0000 0000 0000 0000 0000 0005"), "Value 100000 decimal binary representation");
        Assert.That(FormatDecimalAsBinary(1000000m), Is.EqualTo("0000 0000 0000 0000 0000 0000 000F 4240 0000 0000 0000 0000 0000 0000 0000 0006"), "Value 1000000 decimal binary representation");
        Assert.That(FormatDecimalAsBinary(10000000m), Is.EqualTo("0000 0000 0000 0000 0000 0000 0098 9680 0000 0000 0000 0000 0000 0000 0000 0007"), "Value 10000000 decimal binary representation");
        Assert.That(FormatDecimalAsBinary(100000000m), Is.EqualTo("0000 0000 0000 0000 0000 0000 05F5 E100 0000 0000 0000 0000 0000 0000 0000 0008"), "Value 100000000 decimal binary representation");
        Assert.That(FormatDecimalAsBinary(1000000000m), Is.EqualTo("0000 0000 0000 0000 0000 0000 3B9A CA00 0000 0000 0000 0000 0000 0000 0000 0009"), "Value 1000000000 decimal binary representation");
    }

    [Test]
    public void DecimalBinaryRepresentation_Zero_ShouldShowAllZeros()
    {
        Assert.That(FormatDecimalAsBinary(0m), Is.EqualTo("0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000"), "Zero should be represented as all zeros");
    }

    [Test]
    public void DecimalBinaryRepresentation_MaxValue_ShouldShowCorrectBinary()
    {
        Assert.That(FormatDecimalAsBinary(decimal.MaxValue), Is.EqualTo("FFFFFFFF FFFFFFFF FFFFFFFF 00000000 00000000 00000000 00000000 00000000"), "decimal.MaxValue should show maximum 96-bit mantissa");
    }

    [Test]
    public void DecimalBinaryRepresentation_MinValue_ShouldShowCorrectBinary()
    {
        Assert.That(FormatDecimalAsBinary(decimal.MinValue), Is.EqualTo("FFFFFFFF FFFFFFFF FFFFFFFF 00000000 80000000 00000000 00000000 00000000"), "decimal.MinValue should show negative sign bit set");
    }

    [Test]
    public void DecimalBinaryRepresentation_NegativeOne_ShouldShowNegativeSign()
    {
        Assert.That(FormatDecimalAsBinary(-1m), Is.EqualTo("0000 0000 0000 0000 0000 0000 0000 0001 8000 0000 0000 0000 0000 0000 0000 0000"), "-1 should be represented with negative sign bit");
    }

    [Test]
    public void DecimalBinaryRepresentation_DecimalPlaces_ShouldShowCorrectScale()
    {
        // Test decimal values with different scales
        Assert.That(FormatDecimalAsBinary(1.5m), Is.EqualTo("0000 0000 0000 0000 0000 0000 0000 000F 0000 0000 0000 0000 0000 0000 0000 0001"), "1.5 should have scale of 1");
        Assert.That(FormatDecimalAsBinary(1.25m), Is.EqualTo("0000 0000 0000 0000 0000 0000 0000 007D 0000 0000 0000 0000 0000 0000 0000 0002"), "1.25 should have scale of 2");
        Assert.That(FormatDecimalAsBinary(1.125m), Is.EqualTo("0000 0000 0000 0000 0000 0000 0000 0465 0000 0000 0000 0000 0000 0000 0000 0003"), "1.125 should have scale of 3");
    }

    /// <summary>
    /// Helper method to format a decimal as a binary string with proper spacing
    /// Note: Decimal is 128-bit with specific internal structure
    /// </summary>
    private static string FormatDecimalAsBinary(decimal value)
    {
        // Get the internal representation of decimal
        int[] bits = decimal.GetBits(value);
        
        // Decimal structure: [lo, mid, hi, flags] where flags contains sign and scale
        uint lo = (uint)bits[0];
        uint mid = (uint)bits[1]; 
        uint hi = (uint)bits[2];
        uint flags = (uint)bits[3];
        
        // Format as 128-bit representation with spaces every 8 hex digits for readability
        return $"{hi:X8} {mid:X8} {lo:X8} {flags:X8}";
    }
}