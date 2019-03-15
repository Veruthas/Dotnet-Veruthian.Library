using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public interface IExpandableIndex<K, V> : IExpandableContainer<V>, IExpandableLookup<K, V>
    {
        void AddRange(IEnumerable<V> values);

        void InsertRange(K key, IEnumerable<V> values);
        
        void RemoveRange(K key, int count);
    }

    public interface IExpandableIndex<V> : IExpandableIndex<int, V>, IIndex<V>
    {

    }
}