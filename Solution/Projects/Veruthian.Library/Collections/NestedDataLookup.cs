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

        public int Count
        {
            get
            {
                int count = 0;

                // Should I cache this value?
                foreach (var item in items.Values)
                    if (item.active)
                        count++;

                foreach (var pair in parent.Pairs)
                    if (!items.ContainsKey(pair.Item1))
                        count++;

                return count;
            }
        }

        public void Reset() => items.Clear();

        public void Clear()
        {
            items.Clear();
            parent = null;
        }

        public bool Contains(V value)
        {
            if (value == null)
            {
                foreach (var item in items.Values)
                {
                    if (item.active && item.value == null)
                        return true;
                }

                foreach (var item in parent)
                {
                    if (item == null)
                        return true;
                }
            }
            else
            {
                foreach (var item in items.Values)
                {
                    if (item.active && value.Equals(item.value))
                        return true;
                }

                foreach (var item in parent)
                {
                    if (value.Equals(item))
                        return true;
                }
            }

            return false;
        }


        public bool HasKey(K key)
        {
            if (items.TryGetValue(key, out var item))
            {
                return item.active;
            }
            else
            {
                return parent.HasKey(key);
            }
        }


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

        public IEnumerable<K> Keys
        {
            get
            {
                foreach (var pair in items)
                {
                    if (pair.Value.active)
                        yield return pair.Key;
                }

                foreach (var key in parent.Keys)
                {
                    if (!items.ContainsKey(key))
                        yield return key;
                }
            }
        }

        public IEnumerable<(K, V)> Pairs
        {
            get
            {
                foreach (var pair in items)
                {
                    if (pair.Value.active)
                        yield return (pair.Key, pair.Value.value);
                }

                foreach (var pair in parent.Pairs)
                {
                    if (!items.ContainsKey(pair.Item1))
                        yield return pair;
                }
            }
        }

        public IEnumerator<V> GetEnumerator()
        {
            foreach (var pair in items)
            {
                if (pair.Value.active)
                    yield return pair.Value.value;
            }

            foreach (var pair in parent.Pairs)
            {
                if (!items.ContainsKey(pair.Item1))
                    yield return pair.Item2;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}