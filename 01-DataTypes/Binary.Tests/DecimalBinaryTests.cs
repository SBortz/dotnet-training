using System.Diagnostics;

namespace Binary.Tests;

/// <summary>
/// Tests demonstrating how decimal values are represented in binary format
/// Note: Decimal is a 128-bit value with 96-bit mantissa and 32-bit scale/sign
/// </summary>
public class DecimalBinaryTests
{
    [Test]
    public void DecimalBinaryRepresentation_ShouldShowCorrectBinaryFormat()
    {
        Assert.That(FormatDecimalAsBinary(0m   ), Is.EqualTo("00000000000000000000000000000000 00000000000000000000000000000000 00000000000000000000000000000000 00000000000000000000000000000000"), "Zero should be represented as all zeros");
        Assert.That(FormatDecimalAsBinary(1m   ), Is.EqualTo("00000000000000000000000000000000 00000000000000000000000000000000 00000000000000000000000000000001 00000000000000000000000000000000"), "Value 1 decimal binary representation");
        Assert.That(FormatDecimalAsBinary(10m  ), Is.EqualTo("00000000000000000000000000000000 00000000000000000000000000000000 00000000000000000000000000001010 00000000000000000000000000000000"), "Value 10 decimal binary representation");
        Assert.That(FormatDecimalAsBinary(100m ), Is.EqualTo("00000000000000000000000000000000 00000000000000000000000000000000 00000000000000000000000001100100 00000000000000000000000000000000"), "Value 100 decimal binary representation");
        Assert.That(FormatDecimalAsBinary(-1m  ), Is.EqualTo("00000000000000000000000000000000 00000000000000000000000000000000 00000000000000000000000000000001 10000000000000000000000000000000"), "-1 should be represented with negative sign bit");

        Assert.That(FormatDecimalAsBinary(decimal.MaxValue), Is.EqualTo("11111111111111111111111111111111 11111111111111111111111111111111 11111111111111111111111111111111 00000000000000000000000000000000"), "decimal.MaxValue should show maximum 96-bit mantissa");
        Assert.That(FormatDecimalAsBinary(decimal.MinValue), Is.EqualTo("11111111111111111111111111111111 11111111111111111111111111111111 11111111111111111111111111111111 10000000000000000000000000000000"), "decimal.MinValue should show negative sign bit set");
    }

    [Test]
    public void DecimalBinaryRepresentation_DecimalPlaces_ShouldShowCorrectScale()
    {
        // Test decimal values with different scales - using actual 159-character values from .NET
        Assert.That(FormatDecimalAsBinary(1.5m  ), Is.EqualTo("00000000000000000000000000000000 00000000000000000000000000000000 00000000000000000000000000001111 00000000000000010000000000000000"), "1.5 should have scale of 1");
        Assert.That(FormatDecimalAsBinary(1.25m ), Is.EqualTo("00000000000000000000000000000000 00000000000000000000000000000000 00000000000000000000000001111101 00000000000000100000000000000000"), "1.25 should have scale of 2");
        Assert.That(FormatDecimalAsBinary(1.125m), Is.EqualTo("00000000000000000000000000000000 00000000000000000000000000000000 00000000000000000000010001100101 00000000000000110000000000000000"), "1.125 should have scale of 3");
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
        
        // Convert to binary strings (32 bits each, padded with zeros)
        string hiBinary = Convert.ToString(hi, 2).PadLeft(32, '0');
        string midBinary = Convert.ToString(mid, 2).PadLeft(32, '0');
        string loBinary = Convert.ToString(lo, 2).PadLeft(32, '0');
        string flagsBinary = Convert.ToString(flags, 2).PadLeft(32, '0');

        
        return $"{hiBinary} {midBinary} {loBinary} {flagsBinary}";
    }
}