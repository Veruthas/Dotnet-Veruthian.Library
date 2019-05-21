using System.Collections.Generic;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    /* 
        IContainer<T>
        IExpandableContainer<T> : IContainer<T> => DataSet        
        IPool<A, D> : IContainer<(A, D)>
        
        *IStack<T> : IContainer<T>
        *IQueue<T> : IContainer<T>
        *IDeque<T> : IStack<T>, IQueue<T>        

        ILookup<A, T> : IContainer<T>  => NestedDataLookup, SequentialDataLookup
        IMutableLookup<A, T> : ILookup<A, T>
        IExpandableLookup<A, T> : ILookup<A, T> => DataLookup

        IVector<A, T>: ILookup<A, T> => DataVector
        IMutableVector<A, T>: IVector<A, T>, IMutableLookup<A, T> => DataArray
        IExpandableVector<A, T> : IVector<A, T>, IExpandableLookup<A, T>, IExpandableContainer<T> => DataList
        IOrderedVector<A, T>: IVector<A, T>, IExpandableContainer<T> => SortedDataList*, SortedDataSet*
        
        IVectorLookup<L, V, T> : IVector<V, T>, ILookup<L, T>        
    */

    public interface IContainer<T> : IEnumerable<T>
    {
        Number Count { get; }

        bool Contains(T value);
    }
}