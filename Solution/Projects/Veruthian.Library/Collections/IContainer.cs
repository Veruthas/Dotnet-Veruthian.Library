using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    /* 
        IContainer<T>
        IExpandableContainer<T> : IContainer<T> => DataSet        
        IPool<A, D> : IContainer<(A, V)>

        ILookup<A, V> : IContainer<V>  => NestedDataLookup, SequentialDataLookup
        IMutableLookup<A, V> : ILookup<A, V>
        IExpandableLookup<A, V> : ILookup<A, V> => DataLookup

        IVector<A, V>: ILookup<K, V>
        IMutableVector<A, V>: IVector<A, V>, IMutableLookup<A, V> => DataArray
        IOrderedVector<A, V>: IVector<A, V>, IExpandableContainer<V> => SortedDataList, SortedDataSet
        IExpandableVector<A, V> : IVector<A, V>, IExpandableLookup<A, V>, IExpandableContainer<V> => DataList

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