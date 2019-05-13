using System;
using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Collections
{
    public class DataLookup<A, V> : IMutableLookup<A, V>, IExpandableLookup<A, V>
    {
        Dictionary<A, V> dictionary;


        public DataLookup() => this.dictionary = new Dictionary<A, V>();


        public V this[A address]
        {
            get => dictionary.ContainsKey(address) ? dictionary[address] : throw new KeyNotFoundException();
            set => dictionary[address] = dictionary.ContainsKey(address) ? value : throw new KeyNotFoundException();
        }

        V ILookup<A, V>.this[A address] => this[address];

        public bool TryGet(A address, out V value)
        {
            if (dictionary.ContainsKey(address))
            {
                value = dictionary[address];

                return true;
            }
            else
            {
                value = default(V);

                return false;
            }
        }

        public bool TrySet(A address, V value)
        {
            if (dictionary.ContainsKey(address))
            {
                dictionary[address] = value;

                return true;
            }
            else
            {
                return false;
            }
        }


        public int Count => dictionary.Count;


        public IEnumerable<A> Addresses => dictionary.Keys;

        public IEnumerable<(A, V)> Pairs
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

        public bool HasAddress(A address) => dictionary.ContainsKey(address);

        public void Insert(A address, V value)
        {
            if (dictionary.ContainsKey(address))
                throw new ArgumentException($"Key {address.ToString()} already exists", "key");

            dictionary.Add(address, value);
        }

        public V GetOrInsert(A address, V value)
        {
            if (TryGet(address, out var result))
            {
                return result;
            }
            else
            {
                Insert(address, value);
                return value;
            }
        }

        public void RemoveBy(A address)
        {
            if (!HasAddress(address))
                throw new ArgumentException($"Key {address.ToString()} does not exist", "key");

            dictionary.Remove(address);
        }

        public void Clear() => dictionary.Clear();

        public override string ToString() => Pairs.ToTableString();


    }
}