using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public interface IExpandableIndex<A, V> : IExpandableContainer<V>, IExpandableLookup<A, V>
    {
        void AddRange(IEnumerable<V> values);

        void InsertRange(A address, IEnumerable<V> values);
        
        void RemoveRange(A address, int count);
    }

    public interface IExpandableIndex<V> : IExpandableIndex<int, V>, IIndex<V>
    {

    }
}