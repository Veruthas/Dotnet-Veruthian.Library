using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public interface IResizableVector<A, T> : IResizableContainer<T>, IResizableLookup<A, T>
    {
        void InsertRange(A address, IEnumerable<T> values);
        
        void RemoveRange(A address, int count);
    }

    public interface IResizableVector<V> : IResizableVector<int, V>, IVector<V>
    {

    }
}