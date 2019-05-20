using System;
using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public class NestedDataLookup<A, T> : IMutableLookup<A, T>, IResizableLookup<A, T>
    {
        Dictionary<A, (T value, bool active)> items = new Dictionary<A, (T, bool)>();

        ILookup<A, T> parent;


        public NestedDataLookup() { }

        public NestedDataLookup(ILookup<A, T> parent) => this.parent = parent;


        public ILookup<A, T> Parent => parent;


        public T this[A address]
        {
            get => TryGet(address, out var value) ? value : throw new ArgumentException($"Address {address.ToString()} does not exist", nameof(address));
            set
            {
                if (!TrySet(address, value))
                    throw new ArgumentException($"Address {address.ToString()} does not exist", nameof(address));
            }
        }

        T ILookup<A, T>.this[A address] => this[address];

        public bool TryGet(A address, out T value)
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

            value = default(T);

            return false;
        }

        public bool TrySet(A address, T value)
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

        public Number Count
        {
            get
            {
                var count = Number.Zero;

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

        public bool Contains(T value)
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
                    foreach ((A address, T value) pair in parent.Pairs)
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
                    foreach ((A address, T value) pair in parent.Pairs)
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


        public void Insert(A address, T value)
        {
            if (HasAddress(address))
                throw new ArgumentException($"Address {address.ToString()} already exists", nameof(address));

            items.Add(address, (value, true));
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
            if (this.items.TryGetValue(address, out var value))
            {
                items[address] = (value.value, false);
            }
            else if (this.parent != null)
            {
                if (this.parent.HasAddress(address))
                    items.Add(address, (default(T), false));
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
                    foreach (var address in parent.Addresses)
                    {
                        if (!items.ContainsKey(address))
                            yield return address;
                    }
                }
            }
        }

        public IEnumerable<(A Address, T Value)> Pairs
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

        public IEnumerator<T> GetEnumerator()
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