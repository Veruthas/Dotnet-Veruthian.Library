using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public class SequentialDataLookup<K, V> : ILookup<K, V>
    {
        List<ILookup<K, V>> lookups;


        public SequentialDataLookup() => lookups = new List<ILookup<K, V>>();


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

        public bool Contains(V value)
        {
            foreach (var lookup in lookups)
                if (lookup.Contains(value))
                    return true;

            return false;
        }

        public bool HasKey(K key)
        {
            foreach (var lookup in lookups)
                if (lookup.HasKey(key))
                    return true;

            return false;
        }

        public bool TryGet(K key, out V value)
        {
            foreach (var lookup in lookups)
                if (lookup.TryGet(key, out value))
                    return true;

            value = default(V);

            return false;
        }

        public int Count
        {
            get
            {
                var keys = new HashSet<K>();

                foreach (var lookup in lookups)
                    foreach (var key in lookup.Keys)
                        keys.Add(key);

                return keys.Count;
            }
        }

        public IEnumerable<(K, V)> Pairs
        {
            get
            {
                var keys = new HashSet<K>();

                foreach (var lookup in lookups)
                {
                    foreach ((K Key, V Value) pair in lookup.Pairs)
                    {
                        if (!keys.Contains(pair.Key))
                        {
                            keys.Add(pair.Key);
                            yield return pair;
                        }
                    }
                }
            }
        }

        public IEnumerable<K> Keys
        {
            get
            {
                var keys = new HashSet<K>();

                foreach (var lookup in lookups)
                {
                    foreach (var key in lookup.Keys)
                    {
                        if (!keys.Contains(key))
                        {
                            keys.Add(key);
                            yield return key;
                        }
                    }
                }
            }
        }

        public IEnumerator<V> GetEnumerator()
        {
            var keys = new HashSet<K>();

            foreach (var lookup in lookups)
            {
                foreach ((K Key, V Value) pair in lookup.Pairs)
                {
                    if (!keys.Contains(pair.Key))
                    {
                        keys.Add(pair.Key);
                        
                        yield return pair.Value;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}