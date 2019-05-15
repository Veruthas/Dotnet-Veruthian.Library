using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public struct ReadOnlyLookup<A, V> : ILookup<A, V>
    {
        ILookup<A, V> lookup;


        public ReadOnlyLookup(ILookup<A, V> lookup) => this.lookup = lookup;


        public V this[A address] => lookup[address];

        public int Count => lookup.Count;

        public IEnumerable<A> Addresses => lookup.Addresses;

        public IEnumerable<(A Address, V Value)> Pairs => lookup.Pairs;

        public bool Contains(V value) => lookup.Contains(value);
        
        public bool HasAddress(A address) => lookup.HasAddress(address);

        public bool TryGet(A Address, out V Value) => lookup.TryGet(Address, out Value);
        
        IEnumerator IEnumerable.GetEnumerator() => lookup.GetEnumerator();

        public IEnumerator<V> GetEnumerator() => lookup.GetEnumerator();
    }
}