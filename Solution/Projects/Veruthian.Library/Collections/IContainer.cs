using System.Collections.Generic;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    /* 
        IContainer<T> => FixedContainer
        IResizableContainer<T> : IContainer<T> => DataSet        

        IStack<T> : IContainer<T>
        IPool<A, D> : IContainer<(A, D)>
        
        ILookup<A, T> : IContainer<T>  => FixedLookup, NestedDataLookup, SequentialDataLookup
        IMutableLookup<A, T> : ILookup<A, T>
        IResizableLookup<A, T> : ILookup<A, T> => DataLookup

        IVector<A, T>: ILookup<A, T> => FixedVector, DataString
        IResizableVector<A, T>: IVector<A, T>, IMutableLookup<A, T> => DataVector
        IExpandableVector<A, T> : IVector<A, T>, IExpandableLookup<A, T>, IExpandableContainer<T> => DataList
    */

    public interface IContainer<T> : IEnumerable<T>
    {
        Number Count { get; }

        bool Contains(T value);
    }
}