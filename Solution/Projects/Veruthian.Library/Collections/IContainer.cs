using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    /* 
        IContainer<T>
        IExpandableContainer<T> : IContainer<T> => DataSet        
        IPool<K, A> : IContainer<(K, A)>

        ILookup<K, V> : IContainer<V>  => NestedDataLookup, SequentialDataLookup
        IMutableLookup<K, V> : ILookup<K, V>
        IExpandableLookup<K, V> : ILookup<K, V> => DataLookup

        IIndex<K, V>: ILookup<K, V>
        IMutableIndex<K, V>: IIndex<K, V>, IMutableLookup<K, V> => DataArray
        IOrderedIndex<K, V>: IIndex<K, V>, IExpandableContainer<V> => SortedDataList, SortedDataSet
        IExpandableIndex<K, V> : IIndex<K, V>, IExpandableLookup<K, V>, IExpandableContainer<V> => DataList

        Future:
            IStack<T> : IContainer<T>
            IQueue<T> : IContainer<T>
            IDeque<T> : IStack<T>, IQueue<T>        
    */

    public interface IContainer<T> : IEnumerable<T>
    {
        int Count { get; }

        bool Contains(T value);
    }
}