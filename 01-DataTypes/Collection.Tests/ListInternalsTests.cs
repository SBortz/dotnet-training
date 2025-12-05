using System.Reflection;

namespace Collection.Tests
{
    public class ListInternalsTests
    {
        [Test]
        public void InternalArray_MatchesCapacity_AndIsZeroInitially()
        {
            List<int> l = [];
            int[] items = GetInternalArray(l);

            Assert.That(l.Capacity, Is.EqualTo(0));
            Assert.That(items.Length, Is.EqualTo(0));
        }

        [Test]
        public void AddingItems_UsesInternalArray_AndPreservesValues()
        {
            List<int> l = [1];
            Assert.That(GetInternalArray(l).Length, Is.EqualTo(1));
            
            l.Add(2); Assert.That(GetInternalArray(l).Length, Is.EqualTo(2));
            l.Add(3); Assert.That(GetInternalArray(l).Length, Is.EqualTo(4));
            l.Add(4); Assert.That(GetInternalArray(l).Length, Is.EqualTo(4));
            l.Add(5); Assert.That(GetInternalArray(l).Length, Is.EqualTo(8));
            l.Add(6); Assert.That(GetInternalArray(l).Length, Is.EqualTo(8));
            l.Add(7); Assert.That(GetInternalArray(l).Length, Is.EqualTo(8));
            l.Add(8); Assert.That(GetInternalArray(l).Length, Is.EqualTo(8));
            l.Add(9); Assert.That(GetInternalArray(l).Length, Is.EqualTo(16));
        }

        [Test]
        public void RemovingItemsLeavesCapacity_TrimExcessShrinksCapacity()
        {
            List<int> l = [1, 2, 3, 4, 5, 6, 7, 8, 9];
            
            Assert.That(GetInternalArray(l).Length, Is.EqualTo(9));
            
            // Remove all elements except one, capacity stays the same
            l.RemoveRange(1, 8); Assert.That(GetInternalArray(l).Length, Is.EqualTo(9));
            
            // Trim excess should shrink capacity of the array
            l.TrimExcess(); Assert.That(GetInternalArray(l).Length, Is.EqualTo(1));
        }

        // Helper: privates _items-Feld via Reflection holen
        private static T[] GetInternalArray<T>(List<T> list)
        {
            FieldInfo? fi = typeof(List<T>).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi == null) throw new InvalidOperationException("Field _items nicht gefunden (andere .NET-Version?).");
            return (T[])fi.GetValue(list)!;
        }
    }
}
