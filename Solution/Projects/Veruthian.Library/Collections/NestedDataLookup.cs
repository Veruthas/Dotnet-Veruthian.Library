using System;
using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public class NestedDataLookup<A, V> : IMutableLookup<A, V>, IExpandableLookup<A, V>
    {
        Dictionary<A, (V value, bool active)> items = new Dictionary<A, (V, bool)>();

        ILookup<A, V> parent;


        public NestedDataLookup() { }

        public NestedDataLookup(ILookup<A, V> parent) => this.parent = parent;


        public ILookup<A, V> Parent => parent;


        public V this[A address]
        {
            get => TryGet(address, out var value) ? value : throw new KeyNotFoundException();
            set
            {
                if (!TrySet(address, value))
                    throw new KeyNotFoundException();
            }
        }

        V ILookup<A, V>.this[A address] => this[address];

        public bool TryGet(A address, out V value)
        {
            if (items.TryGetValue(address, out var result))
            {
                if (result.active)
                {
                    value = result.value;

                    return true;
                }
            }
            else if (parent != null)
            {
                return parent.TryGet(address, out value);
            }

            value = default(V);

            return false;
        }

        public bool TrySet(A address, V value)
        {
            if (items.TryGetValue(address, out var result))
            {
                if (result.active)
                {
                    items[address] = (value, true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (parent != null && parent.HasAddress(address))
            {
                items.Add(address, (value, true));
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
                {
                    if (item.active)
                        count++;
                }

                if (parent != null)
                {
                    foreach (var pair in parent.Pairs)
                    {
                        if (!items.ContainsKey(pair.Item1))
                            count++;
                    }
                }

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

                if (parent != null)
                {
                    foreach ((A address, V value) pair in parent.Pairs)
                    {
                        if (!items.ContainsKey(pair.address) && pair.value == null)
                            return true;
                    }
                }
            }
            else
            {
                foreach (var item in items.Values)
                {
                    if (item.active && value.Equals(item.value))
                        return true;
                }

                if (parent != null)
                {
                    foreach ((A address, V value) pair in parent.Pairs)
                    {
                        if (!items.ContainsKey(pair.address) && pair.value.Equals(value))
                            return true;
                    }
                }
            }

            return false;
        }


        public bool HasAddress(A address)
        {
            if (items.TryGetValue(address, out var item))
                return item.active;
            else
                return parent != null && parent.HasAddress(address);
        }


        public void Insert(A address, V value)
        {
            if (HasAddress(address))
                throw new ArgumentException($"Address {address.ToString()} already exists", nameof(address));

            items.Add(address, (value, true));
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
            if (this.items.TryGetValue(address, out var value))
            {
                items[address] = (value.value, false);
            }
            else if (this.parent != null)
            {
                if (this.parent.HasAddress(address))
                    items.Add(address, (default(V), false));
            }
            else
            {
                throw new ArgumentException($"Address {address.ToString()} does not exist", nameof(address));
            }
        }

        public IEnumerable<A> Addresses
        {
            get
            {
                foreach (var pair in items)
                {
                    if (pair.Value.active)
                        yield return pair.Key;
                }

                if (parent != null)
                {
                    foreach (var key in parent.Addresses)
                    {
                        if (!items.ContainsKey(key))
                            yield return key;
                    }
                }
            }
        }

        public IEnumerable<(A, V)> Pairs
        {
            get
            {
                foreach (var pair in items)
                {
                    if (pair.Value.active)
                        yield return (pair.Key, pair.Value.value);
                }

                if (parent != null)
                {
                    foreach (var pair in parent.Pairs)
                    {
                        if (!items.ContainsKey(pair.Item1))
                            yield return pair;
                    }
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

            if (parent != null)
            {
                foreach (var pair in parent.Pairs)
                {
                    if (!items.ContainsKey(pair.Item1))
                        yield return pair.Item2;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}