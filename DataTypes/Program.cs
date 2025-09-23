using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Bitte Größe angeben, z. B. 10MB oder 5GB.");
            return;
        }

        string input = args[0].Trim();
        if (!TryParseSize(input, out long bytesToAllocate))
        {
            Console.WriteLine("Ungültige Größe. Erlaubt sind Suffixe: KB, MB, GB, TB.");
            return;
        }

        int page = Environment.SystemPageSize; // z. B. 4096 unter Linux
        const int chunkSize = 100 * 1024 * 1024; // 100 MB
        var hold = new List<byte[]>();
        long total = 0;

        Console.WriteLine($"Allokiere {bytesToAllocate:N0} Bytes (~{bytesToAllocate / (1024*1024)} MB). PageSize={page}.");

        try
        {
            while (total < bytesToAllocate)
            {
                long remaining = bytesToAllocate - total;
                int thisChunk = (int)Math.Min(chunkSize, remaining);

                var chunk = new byte[thisChunk];
                hold.Add(chunk);

                // Seiten "touchen", damit der Kernel sie wirklich mapped
                uint x = 2463534242; // einfacher PRNG-Seed

                for (int i = 0; i < thisChunk; i += page)
                {
                    // xorshift32 – schneller Pseudozufall pro Seite
                    x ^= x << 13; x ^= x >> 17; x ^= x << 5;

                    int end = Math.Min(thisChunk, i + page);
                    for (int j = i; j < end; j++)
                    {
                        // Fülle die gesamte Seite mit (quasi) zufälligen Bytes
                        chunk[j] = (byte)(x + (uint)(j - i));
                    }
                }

                // Letztes Byte auch noch anfassen, falls nicht exakt auf Seitenende
                chunk[thisChunk - 1] = 1;

                total += thisChunk;
                Console.WriteLine($"Committed: {total / (1024 * 1024)} MB");
            }

            Console.WriteLine("Fertig. Drücke Enter zum Freigeben …");
            Console.ReadLine();
        }
        catch (OutOfMemoryException)
        {
            Console.WriteLine($"OutOfMemory bei ~{total / (1024 * 1024)} MB (RES beobachten).");
            Console.ReadLine();
        }
    }

    static bool TryParseSize(string s, out long bytes)
    {
        bytes = 0;
        s = s.Trim();
        var u = s.ToUpperInvariant();

        long factor = 1;
        string num = u;

        if (u.EndsWith("KB")) { factor = 1L << 10; num = u[..^2]; }
        else if (u.EndsWith("MB")) { factor = 1L << 20; num = u[..^2]; }
        else if (u.EndsWith("GB")) { factor = 1L << 30; num = u[..^2]; }
        else if (u.EndsWith("TB")) { factor = 1L << 40; num = u[..^2]; }

        if (!double.TryParse(num, System.Globalization.NumberStyles.Float,
                             System.Globalization.CultureInfo.InvariantCulture, out var value))
            return false;

        var asLong = (long)(value * factor);
        if (asLong < 0) return false;
        bytes = asLong;
        return true;
    }
}

