using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public interface ILookup<K, V> : IContainer<V>
    {        
        V this[K key] { get; }

        IEnumerable<K> Keys { get; }

        IEnumerable<(K, V)> Pairs { get; }

        bool HasKey(K key);

        bool TryGet(K key, out V value);
    }
}