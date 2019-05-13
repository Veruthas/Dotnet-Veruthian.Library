using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public interface IExpandableIndex<A, T> : IExpandableContainer<T>, IExpandableLookup<A, T>
    {
        void AddRange(IEnumerable<T> values);

        void InsertRange(A address, IEnumerable<T> values);
        
        void RemoveRange(A address, int count);
    }

    public interface IExpandableIndex<V> : IExpandableIndex<int, V>, IVector<V>
    {

    }
}