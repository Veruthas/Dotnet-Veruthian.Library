using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
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
        IExpandableLookup<K, V> : IMutableLookup<K, V> => DataMap

        IIndex<T> : ILookup<int, T>
        IMutableIndex<T> : IIndex<T>, IMutableLookup<int, T> => DataArray
        IOrderedIndex<T> : IIndex<T>, IExpandableContainer<T> => SortedDataList, SortedDataSet
        IExpandableIndex<T> : IMutableIndex<T>, IExpandableLookup<int, T>, IExpandableContainer<T> => DataList
    */

    public interface IContainer<T>
    {
        int Count { get; }

        bool Contains(T value);

        IEnumerable<T> Values { get; }
    }
}