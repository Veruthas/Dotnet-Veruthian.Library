using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Collections
{
    public class DataTable<K, V> : IMutableLookup<K, V>, IExpandableLookup<K, V>
    {
        public V this[K key]
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }

        V ILookup<K, V>.this[K key] => throw new System.NotImplementedException();

        public int Count => throw new System.NotImplementedException();


        public IEnumerable<K> Keys => throw new System.NotImplementedException();

        public IEnumerable<(K, V)> Pairs => throw new System.NotImplementedException();


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

        public bool TryGet(K key, out V value)
        {
            throw new System.NotImplementedException();
        }

        public bool TrySet(K key, V value)
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}