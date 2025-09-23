namespace Collection.Tests.SortImplementations;

public static class QuickSort
{
    public static void Sort(int[] array, int left, int right)
    {
        int i = left, j = right;
        int pivot = array[(left + right) / 2];

        while (i <= j)
        {
            while (array[i] < pivot) i++;
            while (array[j] > pivot) j--;
            if (i <= j)
            {
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
                i++; j--;
            }
        }

        if (left < j) Sort(array, left, j);
        if (i < right) Sort(array, i, right);
    }
}