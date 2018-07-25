using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public class NestedDataLookup<K, V> : IMutableLookup<K, V>, IExpandableLookup<K, V>
    {
        Dictionary<K, (V value, bool active)> items = new Dictionary<K, ( V, bool)>();

        ILookup<K, V> parent;


        public NestedDataLookup(ILookup<K, V> parent) => this.parent = parent;


        public ILookup<K, V> Parent => parent;


        public V this[K key]
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
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
                else
                {
                    value = default(V);

                    return false;
                }
            }

            return parent.TryGet(key, out value);
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
            else if (parent.HasKey(key))
            {
                items.Add(key, (value, true));
            }

            return false;
        }

        public int Count => throw new System.NotImplementedException();

        public void Clear()
        {
            throw new System.NotImplementedException();
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

        public void Insert(K key, V value)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveBy(K key)
        {
            throw new System.NotImplementedException();
        }



        public IEnumerable<K> Keys => throw new System.NotImplementedException();

        public IEnumerable<(K, V)> Pairs => throw new System.NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}