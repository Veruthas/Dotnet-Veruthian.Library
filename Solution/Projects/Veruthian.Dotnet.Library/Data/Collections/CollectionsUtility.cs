using System.Collections.Generic;
using System.IO;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public static class LookupUtility
    {
        // Stream Enumerator
        public static IEnumerator<byte> GetEnumerator(this Stream stream)
        {
            while (true)
            {
                int value = stream.ReadByte();

                if (value != -1)
                    yield return (byte)value;
                else
                    yield break;
            }
        }

        // Adapter
        public static EnumerableAdapter<T> GetEnumerableAdapter<T>(this IEnumerator<T> enumerator)
        {
            return new EnumerableAdapter<T>(enumerator);
        }

        // NotifyingEnumerator
        public static NotifyingEnumerator<T> GetNotifyingEnumerator<T>(this IEnumerator<T> enumerator, EnumeratorMoveNext<T> onMoveNext = null)
        {
            var notifyer = new NotifyingEnumerator<T>(enumerator);

            notifyer.MovedNext += onMoveNext;

            return notifyer;
        }

        public static NotifyingEnumerator<T> GetNotifyingEnumerator<T>(this IEnumerable<T> enumerable, EnumeratorMoveNext<T> onMoveNext = null)
        {
            return GetNotifyingEnumerator(enumerable.GetEnumerator(), onMoveNext);
        }


        // To Array
        public static T[] ToArray<T>(this ILookup<int, T> items)
        {
            var array = new T[items.Count];

            for (int i = 0; i < items.Count; i++)
                array[i] = items[i];

            return array;
        }


        // To Item Array
        public static ItemArray<T> ToItemArray<T>(this T[] items, bool defaultable = false) => new ItemArray<T>(defaultable, items);

        public static ItemArray<T> ToItemArray<T>(this IEnumerable<T> items, bool defaultable = false) => new ItemArray<T>(defaultable, items);

        public static ItemArray<T> ToItemArray<T>(this ILookup<int, T> items, bool defaultable = false) => new ItemArray<T>(defaultable, items);

        public static ItemList<T> ToItemList<T>(this T[] items, bool defaultable = false) => new ItemList<T>(defaultable, items);

        public static ItemList<T> ToItemList<T>(this IEnumerable<T> items, bool defaultable = false) => new ItemList<T>(defaultable, items);

        public static ItemList<T> ToItemList<T>(this ILookup<int, T> items, bool defaultable = false) => new ItemList<T>(defaultable, items);

        // Repeat        
        public static T[] RepeatAsArray<T>(this T value, int times = 1)
        {
            var items = new T[times];

            for (int i = 0; i < times; i++)
                items[i] = value;

            return items;
        }

        public static IEnumerable<T> RepeatAsEnumerable<T>(this T value, int times = 1)
        {
            for (int i = 0; i < times; i++)
                yield return value;
        }

        public static ItemArray<T> RepeatAsItemArray<T>(this T item, int repeated = 1) => new ItemArray<T>(item, repeated);

        public static ItemList<T> RepeatAsItemList<T>(this T item, int repeated = 1) => new ItemList<T>(item, repeated);



        // Add to Lookup
        public static void Add<T>(this IResizeableLookup<int, T> lookup, T item) => lookup.Insert(lookup.Count, item);
    }
}
