using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    /* 
        IContainer<T>
        IExpandableContainer<T> : IContainer<T> => DataSet        
        IPool<A, V> : IContainer<(A, V)>

        ILookup<A, V> : IContainer<V>  => NestedDataLookup, SequentialDataLookup
        IMutableLookup<A, V> : ILookup<A, V>
        IExpandableLookup<A, V> : ILookup<A, V> => DataLookup

        IIndex<A, V>: ILookup<K, V>
        IMutableIndex<A, V>: IIndex<A, V>, IMutableLookup<A, V> => DataArray
        IOrderedIndex<A, V>: IIndex<A, V>, IExpandableContainer<V> => SortedDataList, SortedDataSet
        IExpandableIndex<A, V> : IIndex<A, V>, IExpandableLookup<A, V>, IExpandableContainer<V> => DataList

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