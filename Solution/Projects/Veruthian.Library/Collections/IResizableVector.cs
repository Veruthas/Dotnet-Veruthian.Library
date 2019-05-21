using System.Collections.Generic;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public interface IResizableVector<A, T> : IResizableContainer<T>, IResizableLookup<A, T>, IMutableVector<A, T>
        where A : ISequential<A>
    {
        void InsertRange(A address, IEnumerable<T> values);

        void RemoveRange(A address, Number amount);
    }

    public interface IResizableVector<V> : IResizableVector<Number, V>, IVector<V>
    {

    }
}