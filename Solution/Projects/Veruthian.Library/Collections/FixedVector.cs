using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public struct FixedVector<A, T> : IVector<A, T>
        where A : ISequential<A>
    {
        private IVector<A, T> vector;

        public FixedVector(IVector<A, T> vector) => this.vector = vector;


        public T this[A address] => vector[address];

        public A Start => vector.Start;

        public IEnumerable<A> Addresses => vector.Addresses;

        public IEnumerable<(A Address, T Value)> Pairs => vector.Pairs;

        public Number Count => vector.Count;

        public bool Contains(T value) => vector.Contains(value);

        public A GetAddress(Number offset) => vector.GetAddress(offset);

        public IEnumerator<T> GetEnumerator() => vector.GetEnumerator();

        public bool IsValidAddress(A address) => vector.IsValidAddress(address);

        public bool TryGet(A Address, out T Value) => vector.TryGet(Address, out Value);

        IEnumerator IEnumerable.GetEnumerator() => vector.GetEnumerator();

        public override string ToString() => vector.ToListString();
    }

    public struct FixedVector<T> : IVector<T>
    {
        private IVector<T> vector;

        public FixedVector(IVector<T> vector) => this.vector = vector;

        public T this[Number address] => vector[address];

        public Number Start => vector.Start;

        public IEnumerable<Number> Addresses => vector.Addresses;

        public IEnumerable<(Number Address, T Value)> Pairs => vector.Pairs;

        public Number Count => vector.Count;

        public bool Contains(T value) => vector.Contains(value);

        public Number GetAddress(Number offset) => vector.GetAddress(offset);

        public IEnumerator<T> GetEnumerator() => vector.GetEnumerator();

        public bool IsValidAddress(Number address) => vector.IsValidAddress(address);

        public bool TryGet(Number Address, out T Value) => vector.TryGet(Address, out Value);

        IEnumerator IEnumerable.GetEnumerator() => vector.GetEnumerator();

        public override string ToString() => vector.ToListString();
    }
}