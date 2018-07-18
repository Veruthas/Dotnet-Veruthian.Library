using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Collections
{
    /* 
        IContainer<T>
        IExpandableContainer<T> : IContainer<T> => DataSet
        Future:
        IStack<T> : IContainer<T>
        IQueue<T> : IContainer<T>
        IDeque<T> : IStack<T>, IQueue<T>        

        ILookup<K, V> : IContainer<V>  => NestedDataMap, SequentialDataMap
        IMutableLookup<K, V> : ILookup<K, V>
        IExpandableLookup<K, V> : ILookup<K, V> => DataMap

        IIndex<K, V>: ILookup<K, V>
        IMutableIndex<K, V>: IIndex<K, V>, IMutableLookup<K, V> => DataArray
        IOrderedIndex<K, V>: IIndex<K, V>, IExpandableContainer<V> => SortedDataList, SortedDataSet
        IExpandableIndex<K, V> : IIndex<K, V>, IExpandableLookup<K, V>, IExpandableContainer<V> => DataList
    */

    public interface IContainer<T> : IEnumerable<T>
    {
        int Count { get; }

        bool Contains(T value);
    }
}