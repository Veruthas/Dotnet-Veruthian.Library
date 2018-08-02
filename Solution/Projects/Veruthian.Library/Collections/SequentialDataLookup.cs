using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public class SequentialDataLookup<K, V> : ILookup<K, V>
    {
        List<ILookup<K, V>> lookups;

        public V this[K key]
        {
            get
            {
                foreach (var lookup in lookups)
                {
                    if (lookup.TryGet(key, out var value))
                        return value;
                }

                throw new KeyNotFoundException();
            }
        }

        public IEnumerable<K> Keys => throw new System.NotImplementedException();

        public IEnumerable<(K, V)> Pairs => throw new System.NotImplementedException();

        public int Count
        {
            get
            {
                return 0;
            }
        }

        public bool Contains(V value)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<V> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public bool HasKey(K key)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGet(K key, out V value)
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}