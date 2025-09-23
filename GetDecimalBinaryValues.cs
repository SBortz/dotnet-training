using System;
using System.Linq;

public class GetDecimalBinaryValues
{
    public static void Main()
    {
        Console.WriteLine("=== Decimal Binary Values (159 characters each) ===");
        Console.WriteLine($"0m = \"{FormatDecimalAsBinary(0m)}\"");
        Console.WriteLine($"1m = \"{FormatDecimalAsBinary(1m)}\"");
        Console.WriteLine($"10m = \"{FormatDecimalAsBinary(10m)}\"");
        Console.WriteLine($"100m = \"{FormatDecimalAsBinary(100m)}\"");
        Console.WriteLine($"1000m = \"{FormatDecimalAsBinary(1000m)}\"");
        Console.WriteLine($"10000m = \"{FormatDecimalAsBinary(10000m)}\"");
        Console.WriteLine($"100000m = \"{FormatDecimalAsBinary(100000m)}\"");
        Console.WriteLine($"1000000m = \"{FormatDecimalAsBinary(1000000m)}\"");
        Console.WriteLine($"10000000m = \"{FormatDecimalAsBinary(10000000m)}\"");
        Console.WriteLine($"100000000m = \"{FormatDecimalAsBinary(100000000m)}\"");
        Console.WriteLine($"1000000000m = \"{FormatDecimalAsBinary(1000000000m)}\"");
        Console.WriteLine($"decimal.MaxValue = \"{FormatDecimalAsBinary(decimal.MaxValue)}\"");
        Console.WriteLine($"decimal.MinValue = \"{FormatDecimalAsBinary(decimal.MinValue)}\"");
        Console.WriteLine($"-1m = \"{FormatDecimalAsBinary(-1m)}\"");
        Console.WriteLine($"1.5m = \"{FormatDecimalAsBinary(1.5m)}\"");
        Console.WriteLine($"1.25m = \"{FormatDecimalAsBinary(1.25m)}\"");
        Console.WriteLine($"1.125m = \"{FormatDecimalAsBinary(1.125m)}\"");
        
        // Show string length to confirm
        Console.WriteLine($"\nString length: {FormatDecimalAsBinary(1m).Length}");
    }
    
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
        
        // Add spaces every 4 bits for readability (like int32 tests)
        string hiFormatted = string.Join(" ", Enumerable.Range(0, 8).Select(i => hiBinary.Substring(i * 4, 4)));
        string midFormatted = string.Join(" ", Enumerable.Range(0, 8).Select(i => midBinary.Substring(i * 4, 4)));
        string loFormatted = string.Join(" ", Enumerable.Range(0, 8).Select(i => loBinary.Substring(i * 4, 4)));
        string flagsFormatted = string.Join(" ", Enumerable.Range(0, 8).Select(i => flagsBinary.Substring(i * 4, 4)));
        
        return $"{hiFormatted} {midFormatted} {loFormatted} {flagsFormatted}";
    }
}