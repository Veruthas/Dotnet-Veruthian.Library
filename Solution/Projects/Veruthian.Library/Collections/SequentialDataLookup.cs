using System;
using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public class SequentialDataLookup<A, T> : ILookup<A, T>
    {
        List<ILookup<A, T>> lookups;


        public SequentialDataLookup() => lookups = new List<ILookup<A, T>>();


        public T this[A address]
        {
            get
            {
                foreach (var lookup in lookups)
                {
                    if (lookup.TryGet(address, out var value))
                        return value;
                }

                throw new ArgumentException($"Address {address.ToString()} does not exist", nameof(address));
            }
        }

        public bool Contains(T value)
        {
            foreach (var lookup in lookups)
                if (lookup.Contains(value))
                    return true;

            return false;
        }

        public bool HasAddress(A address)
        {
            foreach (var lookup in lookups)
                if (lookup.HasAddress(address))
                    return true;

            return false;
        }

        public bool TryGet(A address, out T value)
        {
            foreach (var lookup in lookups)
                if (lookup.TryGet(address, out value))
                    return true;

            value = default(T);

            return false;
        }

        public Numeric.Number Count
        {
            get
            {
                var addresses = new HashSet<A>();

                foreach (var lookup in lookups)
                    foreach (var address in lookup.Addresses)
                        addresses.Add(address);

                return addresses.Count;
            }
        }

        public IEnumerable<(A Address, T Value)> Pairs
        {
            get
            {
                var addresses = new HashSet<A>();

                foreach (var lookup in lookups)
                {
                    foreach ((A Address, T Value) pair in lookup.Pairs)
                    {
                        if (!addresses.Contains(pair.Address))
                        {
                            addresses.Add(pair.Address);
                            yield return pair;
                        }
                    }
                }
            }
        }

        public IEnumerable<A> Addresses
        {
            get
            {
                var addresses = new HashSet<A>();

                foreach (var lookup in lookups)
                {
                    foreach (var address in lookup.Addresses)
                    {
                        if (!addresses.Contains(address))
                        {
                            addresses.Add(address);
                            yield return address;
                        }
                    }
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var addresses = new HashSet<A>();

            foreach (var lookup in lookups)
            {
                foreach ((A Address, T Value) pair in lookup.Pairs)
                {
                    if (!addresses.Contains(pair.Address))
                    {
                        addresses.Add(pair.Address);
                        
                        yield return pair.Value;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}