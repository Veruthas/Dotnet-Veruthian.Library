namespace Veruthian.Library.Collections.Extensions
{
    public static class CollectionExtensions
    {
        public static DataIndex<T> ToDataIndex<T>(this IContainer<T> items) => DataIndex<T>.Extract(items, items.Count);

        public static DataArray<T> ToDataArray<T>(this IContainer<T> items) => DataArray<T>.Extract(items, items.Count);

        public static DataList<T> ToDataList<T>(this IContainer<T> items) => new DataList<T>(items);

    }
}