using System;
using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public class DataLookup<A, T> : IMutableLookup<A, T>, IResizableLookup<A, T>
    {
        Dictionary<A, T> dictionary;


        public DataLookup() => this.dictionary = new Dictionary<A, T>();


        public T this[A address]
        {
            get => dictionary.ContainsKey(address) ? dictionary[address] : throw new ArgumentException($"Address {address.ToString()} does not exist", nameof(address));
            set => dictionary[address] = dictionary.ContainsKey(address) ? value : throw new ArgumentException($"Address {address.ToString()} does not exist", nameof(address));
        }

        T ILookup<A, T>.this[A address] => this[address];

        public bool TryGet(A address, out T value)
        {
            if (dictionary.ContainsKey(address))
            {
                value = dictionary[address];

                return true;
            }
            else
            {
                value = default(T);

                return false;
            }
        }

        public bool TrySet(A address, T value)
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


        public Number Count => dictionary.Count;


        public IEnumerable<A> Addresses => dictionary.Keys;

        public IEnumerable<(A Address, T Value)> Pairs
        {
            get
            {
                foreach (var pair in dictionary)
                    yield return (pair.Key, pair.Value);
            }
        }

        public IEnumerator<T> GetEnumerator() => dictionary.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        public bool Contains(T value) => dictionary.ContainsValue(value);

        public bool HasAddress(A address) => dictionary.ContainsKey(address);

        public void Insert(A address, T value)
        {
            if (dictionary.ContainsKey(address))
                throw new ArgumentException($"Address {address.ToString()} already exists", nameof(address));

            dictionary.Add(address, value);
        }

        public T GetOrInsert(A address, T value)
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
                throw new ArgumentException($"Address {address.ToString()} does not exist", nameof(address));

            dictionary.Remove(address);
        }

        public void Clear() => dictionary.Clear();

        public override string ToString() => Pairs.ToTableString();


    }
}