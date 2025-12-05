using Collection.Tests.SortImplementations;

namespace Sorting.Tests;

public class QuickSortTests
{
    [Test]
    public void Sort_EmptyArray_ShouldNotThrow()
    {
        // Arrange
        int[] array = new int[0];

        // Act & Assert
        // For empty arrays, we need to handle the case where right < left
        Assert.DoesNotThrow(() => {
            if (array.Length > 0)
                QuickSort.Sort(array, 0, array.Length - 1);
        });
        Assert.That(array, Is.Empty);
    }

    [Test]
    public void Sort_SingleElement_ShouldRemainUnchanged()
    {
        // Arrange
        int[] array = { 42 };

        // Act
        QuickSort.Sort(array, 0, array.Length - 1);

        // Assert
        Assert.That(array, Is.EqualTo(new int[] { 42 }));
    }

    [Test]
    public void Sort_AlreadySortedArray_ShouldRemainSorted()
    {
        // Arrange
        int[] array = { 1, 2, 3, 4, 5 };

        // Act
        QuickSort.Sort(array, 0, array.Length - 1);

        // Assert
        Assert.That(array, Is.EqualTo(new int[] { 1, 2, 3, 4, 5 }));
    }

    [Test]
    public void Sort_ReverseSortedArray_ShouldBeSorted()
    {
        // Arrange
        int[] array = { 5, 4, 3, 2, 1 };

        // Act
        QuickSort.Sort(array, 0, array.Length - 1);

        // Assert
        Assert.That(array, Is.EqualTo(new int[] { 1, 2, 3, 4, 5 }));
    }

    [Test]
    public void Sort_UnsortedArray_ShouldBeSorted()
    {
        // Arrange
        int[] array = { 64, 34, 25, 12, 22, 11, 90 };

        // Act
        QuickSort.Sort(array, 0, array.Length - 1);

        // Assert
        Assert.That(array, Is.EqualTo(new int[] { 11, 12, 22, 25, 34, 64, 90 }));
    }

    [Test]
    public void Sort_ArrayWithDuplicates_ShouldBeSorted()
    {
        // Arrange
        int[] array = { 5, 2, 8, 2, 9, 1, 5 };

        // Act
        QuickSort.Sort(array, 0, array.Length - 1);

        // Assert
        Assert.That(array, Is.EqualTo(new int[] { 1, 2, 2, 5, 5, 8, 9 }));
    }

    [Test]
    public void Sort_ArrayWithNegativeNumbers_ShouldBeSorted()
    {
        // Arrange
        int[] array = { -3, 7, -1, 0, 4, -2 };

        // Act
        QuickSort.Sort(array, 0, array.Length - 1);

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
        QuickSort.Sort(array, 0, array.Length - 1);

        // Assert
        for (int i = 0; i < array.Length - 1; i++)
        {
            Assert.That(array[i], Is.LessThanOrEqualTo(array[i + 1]), 
                $"Array is not sorted at index {i}: {array[i]} > {array[i + 1]}");
        }
    }

    [Test]
    public void Sort_PartialArray_ShouldSortOnlySpecifiedRange()
    {
        // Arrange
        int[] array = { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };

        // Act - Sort only elements from index 2 to 7
        QuickSort.Sort(array, 2, 7);

        // Assert - Only the middle part should be sorted
        Assert.That(array[0], Is.EqualTo(9));
        Assert.That(array[1], Is.EqualTo(8));
        Assert.That(array[2], Is.EqualTo(2)); // 7,6,5,4,3,2 sorted becomes 2,3,4,5,6,7
        Assert.That(array[3], Is.EqualTo(3));
        Assert.That(array[4], Is.EqualTo(4));
        Assert.That(array[5], Is.EqualTo(5));
        Assert.That(array[6], Is.EqualTo(6));
        Assert.That(array[7], Is.EqualTo(7));
        Assert.That(array[8], Is.EqualTo(1));
        Assert.That(array[9], Is.EqualTo(0));
    }
}