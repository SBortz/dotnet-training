using Collection.Tests.SortImplementations;

namespace Sorting.Tests;

public class SortingPerformanceTests
{
    [Test]
    public void BubbleSortVsQuickSort_PerformanceComparison_MediumArray()
    {
        // Arrange - Medium array to show more significant difference
        int[] bubbleArray = new int[10000];
        int[] quickArray = new int[10000];
        Random random = new Random(42);
        
        for (int i = 0; i < 10000; i++)
        {
            int value = random.Next(-10000, 10000);
            bubbleArray[i] = value;
            quickArray[i] = value;
        }

        // Act & Measure
        var bubbleStopwatch = System.Diagnostics.Stopwatch.StartNew();
        BubbleSort.Sort(bubbleArray);
        bubbleStopwatch.Stop();

        var quickStopwatch = System.Diagnostics.Stopwatch.StartNew();
        QuickSort.Sort(quickArray, 0, quickArray.Length - 1);
        quickStopwatch.Stop();

        // Assert
        Assert.That(bubbleArray, Is.EqualTo(quickArray), "Both algorithms should produce the same sorted result");
        
        Console.WriteLine($"=== Medium Array (10,000 elements) ===");
        Console.WriteLine($"BubbleSort: {bubbleStopwatch.ElapsedMilliseconds}ms");
        Console.WriteLine($"QuickSort: {quickStopwatch.ElapsedMilliseconds}ms");
        Console.WriteLine($"Speedup: {(double)bubbleStopwatch.ElapsedMilliseconds / quickStopwatch.ElapsedMilliseconds:F2}x");
        
        // QuickSort should be significantly faster for this size
        Assert.That(quickStopwatch.ElapsedMilliseconds, Is.LessThan(bubbleStopwatch.ElapsedMilliseconds), 
            "QuickSort should be faster than BubbleSort for 10,000 elements");
    }

}