using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public struct FixedLookup<A, V> : ILookup<A, V>
    {
        ILookup<A, V> lookup;


        public FixedLookup(ILookup<A, V> lookup) => this.lookup = lookup;


        public V this[A address] => lookup[address];

        public Number Count => lookup.Count;
        

        public IEnumerable<A> Addresses => lookup.Addresses;

        public IEnumerable<(A Address, V Value)> Pairs => lookup.Pairs;

        public bool Contains(V value) => lookup.Contains(value);
        
        public bool IsValidAddress(A address) => lookup.IsValidAddress(address);

        public bool TryGet(A Address, out V Value) => lookup.TryGet(Address, out Value);
        
        IEnumerator IEnumerable.GetEnumerator() => lookup.GetEnumerator();

        public IEnumerator<V> GetEnumerator() => lookup.GetEnumerator();
    }
}