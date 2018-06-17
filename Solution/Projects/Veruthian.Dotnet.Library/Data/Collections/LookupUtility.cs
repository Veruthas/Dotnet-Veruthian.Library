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


        public static ItemArray<T> ToItemArray<T>(this T item, int repeated = 1) => new ItemArray<T>(item, repeated);

        public static ItemList<T> ToItemList<T>(this T item, int repeated = 1) => new ItemList<T>(item, repeated);
    }
}
