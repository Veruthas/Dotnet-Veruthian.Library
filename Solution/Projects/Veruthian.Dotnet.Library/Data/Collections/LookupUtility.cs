using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public static class LookupUtility
    {
        public static void Add<T>(this IResizeableLookup<int, T> lookup, T item) => lookup.Insert(lookup.Count, item);

        public static ItemArray<T> ToItemArray<T>(this T[] items) => new ItemArray<T>(items);

        public static ItemList<T> ToItemList<T>(this T[] items) => new ItemList<T>(items);


        public static ItemArray<T> ToItemArray<T>(this IEnumerable<T> items) => new ItemArray<T>(items);

        public static ItemList<T> ToItemList<T>(this IEnumerable<T> items) => new ItemList<T>(items);


        public static ItemArray<T> ToItemArray<T>(this ILookup<int, T> items) => new ItemArray<T>(items);

        public static ItemList<T> ToItemList<T>(this ILookup<int, T> items) => new ItemList<T>(items);


        public static ItemArray<T> RepeatAsItemArray<T>(this T item, int repeated = 1) => new ItemArray<T>(item, repeated);

        public static ItemList<T> RepeatAsItemList<T>(this T item, int repeated = 1) => new ItemList<T>(item, repeated);


        public static T[] ToArray<T>(this ILookup<int, T> items)
        {
            var array = new T[items.Count];

            for (int i = 0; i < items.Count; i++)
                array[i] = items[i];

            return array;
        }

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
    }
}
