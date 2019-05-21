using System.Collections.Generic;

namespace Veruthian.Library.Collections.Extensions
{
    public static class CollectionExtensions
    {
        public static DataVector<T> ToDataVector<T>(this IContainer<T> items) => DataVector<T>.Withdraw(items, items.Count);

        public static DataVector<T> ToDataArray<T>(this IContainer<T> items) => DataVector<T>.Withdraw(items, items.Count.ToCheckedSignedInt());
        


        public static DataVector<T> ToDataVector<T>(this IEnumerable<T> items) => DataVector<T>.Withdraw(items);

        public static DataVector<T> ToDataArray<T>(this IEnumerable<T> items) => DataVector<T>.Withdraw(items);


        public static DataVector<T> ToDataIndex<T>(this T[] items) => DataVector<T>.From(items);

        public static DataVector<T> ToDataArray<T>(this T[] items) => DataVector<T>.From(items);        
    }
}