using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Collections
{
    public interface ILookup<K, V> : IContainer<V>
    {        
        V this[K key] { get; }

        IEnumerable<K> Keys { get; }

        IEnumerable<KeyValuePair<K, V>> Pairs { get; }

        bool HasKey(K key);

        bool TryGet(K key, out V value);
    }
}