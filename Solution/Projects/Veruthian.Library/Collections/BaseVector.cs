using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public abstract class BaseVector<T> : IVector<T>, IEnumerable<T>
    {

        public Number Start => 0;

        public abstract Number Count { get; }

        public T this[Number address]
        {
            get
            {
                VerifyIndex(address);

                return RawGet(address);
            }
        }

        protected abstract T RawGet(Number verifiedAddress);

        public bool TryGet(Number address, out T value)
        {
            if (IsValidAddress(address))
            {
                value = RawGet(address);
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public abstract bool Contains(T value);


        protected void VerifyIndex(Number address)
        {
            if (!IsValidAddress(address))
                throw new ArgumentOutOfRangeException(nameof(address));
        }

        protected bool IsValidAddress(Number address) => (uint)address < Count;

        bool ILookup<Number, T>.HasAddress(Number address) => IsValidAddress(address);


        IEnumerable<Number> ILookup<Number, T>.Addresses => Enumerables.GetRange(Number.Zero, Count- 1);

        public IEnumerable<(Number Address, T Value)> Pairs
        {
            get
            {
                for (var i = Number.Zero; i < Count; i++)
                    yield return (i, RawGet(i));
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = Number.Zero; i < Count; i++)
                yield return RawGet(i);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        public override string ToString() => this.ToListString();
    }
}