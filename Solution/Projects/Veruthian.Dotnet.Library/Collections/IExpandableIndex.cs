using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Collections
{
    public interface IExpandableIndex<K, V> : IMutableIndex<K, V>, IExpandableContainer<V>, IExpandableLookup<K, V>
    {
        void AddRange(IEnumerable<V> values);

        void InsertRange(K key, IEnumerable<V> values);
        
        void RemoveRange(K key, int count);
    }
}