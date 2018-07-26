using System;
using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public class NestedDataLookup<K, V> : IMutableLookup<K, V>, IExpandableLookup<K, V>
    {
        Dictionary<K, (V value, bool active)> items = new Dictionary<K, (V, bool)>();

        ILookup<K, V> parent;


        public NestedDataLookup(ILookup<K, V> parent) => this.parent = parent;


        public ILookup<K, V> Parent => parent;


        public V this[K key]
        {
            get => TryGet(key, out var value) ? value : throw new KeyNotFoundException();
            set
            {
                if (!TrySet(key, value))
                    throw new KeyNotFoundException();
            }
        }

        V ILookup<K, V>.this[K key] => this[key];

        public bool TryGet(K key, out V value)
        {
            if (items.TryGetValue(key, out var result))
            {
                if (result.active)
                {
                    value = result.value;

                    return true;
                }
            }
            else if (parent != null)
            {
                return parent.TryGet(key, out value);
            }

            value = default(V);

            return false;
        }

        public bool TrySet(K key, V value)
        {
            if (items.TryGetValue(key, out var result))
            {
                if (result.active)
                {
                    items[key] = (value, true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (parent != null && parent.HasKey(key))
            {
                items.Add(key, (value, true));
            }

            return false;
        }

        public int Count => items.Count;

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(V value)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<V> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public bool HasKey(K key) => this.items.ContainsKey(key) || this.parent.HasKey(key);

        public void Insert(K key, V value)
        {
            if (HasKey(key))
                throw new ArgumentException($"Key {key.ToString()} already exists", "key");

            items.Add(key, (value, true));
        }

        public void RemoveBy(K key)
        {
            if (this.items.TryGetValue(key, out var value))
            {
                items[key] = (value.value, false);
            }
            else if (this.parent != null)
            {
                if (this.parent.HasKey(key))
                    items.Add(key, (default(V), false));
            }
            else
            {
                throw new ArgumentException($"Key {key.ToString()} does not exist", "key");
            }
        }

        public IEnumerable<K> Keys => throw new System.NotImplementedException();

        public IEnumerable<(K, V)> Pairs => throw new System.NotImplementedException();

        public IEnumerable<(K, V)> UniquePairs
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}