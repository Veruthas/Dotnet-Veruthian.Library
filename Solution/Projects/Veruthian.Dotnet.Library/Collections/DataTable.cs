using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Collections
{
    public class DataTable<K, V> : IExpandableLookup<K, V>
    {
        V ILookup<K, V>.this[K key] => throw new System.NotImplementedException();

        IEnumerable<K> ILookup<K, V>.Keys => throw new System.NotImplementedException();

        IEnumerable<(K, V)> ILookup<K, V>.Pairs => throw new System.NotImplementedException();

        int IContainer<V>.Count => throw new System.NotImplementedException();

        IEnumerable<V> IContainer<V>.Values => throw new System.NotImplementedException();

        void IExpandableLookup<K, V>.Clear()
        {
            throw new System.NotImplementedException();
        }

        bool IContainer<V>.Contains(V value)
        {
            throw new System.NotImplementedException();
        }

        bool ILookup<K, V>.HasKey(K key)
        {
            throw new System.NotImplementedException();
        }

        void IExpandableLookup<K, V>.Insert(K key, V value)
        {
            throw new System.NotImplementedException();
        }

        void IExpandableLookup<K, V>.RemoveBy(K key)
        {
            throw new System.NotImplementedException();
        }

        bool ILookup<K, V>.TryGet(K key, out V value)
        {
            throw new System.NotImplementedException();
        }
    }
}