using Collection.Tests.SortImplementations;

namespace Sorting.Tests;

public class BubbleSortTests
{
    [Test]
    public void Sort_EmptyArray_ShouldNotThrow()
    {
        // Arrange
        int[] array = new int[0];

        // Act & Assert
        Assert.DoesNotThrow(() => BubbleSort.Sort(array));
        Assert.That(array, Is.Empty);
    }

    [Test]
    public void Sort_SingleElement_ShouldRemainUnchanged()
    {
        // Arrange
        int[] array = { 42 };

        // Act
        BubbleSort.Sort(array);

        // Assert
        Assert.That(array, Is.EqualTo(new int[] { 42 }));
    }

    [Test]
    public void Sort_AlreadySortedArray_ShouldRemainSorted()
    {
        // Arrange
        int[] array = { 1, 2, 3, 4, 5 };

        // Act
        BubbleSort.Sort(array);

        // Assert
        Assert.That(array, Is.EqualTo(new int[] { 1, 2, 3, 4, 5 }));
    }

    [Test]
    public void Sort_ReverseSortedArray_ShouldBeSorted()
    {
        // Arrange
        int[] array = { 5, 4, 3, 2, 1 };

        // Act
        BubbleSort.Sort(array);

        // Assert
        Assert.That(array, Is.EqualTo(new int[] { 1, 2, 3, 4, 5 }));
    }

    [Test]
    public void Sort_UnsortedArray_ShouldBeSorted()
    {
        // Arrange
        int[] array = { 64, 34, 25, 12, 22, 11, 90 };

        // Act
        BubbleSort.Sort(array);

        // Assert
        Assert.That(array, Is.EqualTo(new int[] { 11, 12, 22, 25, 34, 64, 90 }));
    }

    [Test]
    public void Sort_ArrayWithDuplicates_ShouldBeSorted()
    {
        // Arrange
        int[] array = { 5, 2, 8, 2, 9, 1, 5 };

        // Act
        BubbleSort.Sort(array);

        // Assert
        Assert.That(array, Is.EqualTo(new int[] { 1, 2, 2, 5, 5, 8, 9 }));
    }

    [Test]
    public void Sort_ArrayWithNegativeNumbers_ShouldBeSorted()
    {
        // Arrange
        int[] array = { -3, 7, -1, 0, 4, -2 };

        // Act
        BubbleSort.Sort(array);

        // Assert
        Assert.That(array, Is.EqualTo(new int[] { -3, -2, -1, 0, 4, 7 }));
    }

    [Test]
    public void Sort_LargeArray_ShouldBeSorted()
    {
        // Arrange
        int[] array = new int[1000];
        Random random = new Random(42);
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = random.Next(-1000, 1000);
        }

        // Act
        BubbleSort.Sort(array);

        // Assert
        for (int i = 0; i < array.Length - 1; i++)
        {
            Assert.That(array[i], Is.LessThanOrEqualTo(array[i + 1]), 
                $"Array is not sorted at index {i}: {array[i]} > {array[i + 1]}");
        }
    }
}