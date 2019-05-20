using System.Collections.Generic;

namespace Veruthian.Library.Collections.Extensions
{
    public static class CollectionExtensions
    {
        public static DataVector<T> ToDataIndex<T>(this IContainer<T> items) => DataVector<T>.Extract(items, items.Count);

        public static DataArray<T> ToDataArray<T>(this IContainer<T> items) => DataArray<T>.Extract(items, items.Count.ToCheckedSignedInt());

        public static DataList<T> ToDataList<T>(this IContainer<T> items) => DataList<T>.Extract(items);


        public static DataVector<T> ToDataIndex<T>(this IEnumerable<T> items) => DataVector<T>.Extract(items);

        public static DataArray<T> ToDataArray<T>(this IEnumerable<T> items) => DataArray<T>.Extract(items);

        public static DataList<T> ToDataList<T>(this IEnumerable<T> items) => DataList<T>.Extract(items);


        public static DataVector<T> ToDataIndex<T>(this T[] items) => DataVector<T>.From(items);

        public static DataArray<T> ToDataArray<T>(this T[] items) => DataArray<T>.From(items);

        public static DataList<T> ToDataList<T>(this T[] items) => DataList<T>.From(items);
    }
}