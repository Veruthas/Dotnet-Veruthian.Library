using System;
using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Collections
{
    public class DataLookup<K, V> : IMutableLookup<K, V>, IExpandableLookup<K, V>
    {
        Dictionary<K, V> dictionary;


        public DataLookup() => this.dictionary = new Dictionary<K, V>();


        public V this[K key]
        {
            get => dictionary.ContainsKey(key) ? dictionary[key] : throw new KeyNotFoundException();
            set => dictionary[key] = dictionary.ContainsKey(key) ? value : throw new KeyNotFoundException();
        }

        V ILookup<K, V>.this[K key] => this[key];

        public bool TryGet(K key, out V value)
        {
            if (dictionary.ContainsKey(key))
            {
                value = dictionary[key];

                return true;
            }
            else
            {
                value = default(V);

                return false;
            }
        }

        public bool TrySet(K key, V value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;

                return true;
            }
            else
            {
                return false;
            }
        }


        public int Count => dictionary.Count;


        public IEnumerable<K> Keys => dictionary.Keys;

        public IEnumerable<(K, V)> Pairs
        {
            get
            {
                foreach (var pair in dictionary)
                    yield return (pair.Key, pair.Value);
            }
        }

        public IEnumerator<V> GetEnumerator() => dictionary.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        public bool Contains(V value) => dictionary.ContainsValue(value);

        public bool HasKey(K key) => dictionary.ContainsKey(key);

        public void Insert(K key, V value)
        {
            if (dictionary.ContainsKey(key))
                throw new ArgumentException($"Key {key.ToString()} already exists", "key");

            dictionary.Add(key, value);
        }

        public V GetOrInsert(K key, V value)
        {
            if (TryGet(key, out var result))
            {
                return result;
            }
            else
            {
                Insert(key, value);
                return value;
            }
        }

        public void RemoveBy(K key)
        {
            if (!HasKey(key))
                throw new ArgumentException($"Key {key.ToString()} does not exist", "key");

            dictionary.Remove(key);
        }

        public void Clear() => dictionary.Clear();

        public override string ToString() => Pairs.ToTableString();


    }
}