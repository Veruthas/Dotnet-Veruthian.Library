using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public struct FixedVector<T> : IVector<T>, IEnumerable<T>
    {
        IVector<T> vector;

        Number offset;

        Number count;

        Number start;


        public FixedVector(IVector<T> vector)
        {
            this.vector = vector;

            this.offset = 0;

            this.count = vector.Count;

            this.start = 0;
        }

        public FixedVector(IVector<T> vector, Number offset)
        {
            this.vector = vector;

            this.offset = offset;

            this.count = vector.Count - offset;

            this.start = 0;
        }

        public FixedVector(IVector<T> vector, Number offset, Number count)
        {
            this.vector = vector;

            this.offset = offset;

            this.count = count;

            this.start = 0;
        }

        public FixedVector(IVector<T> vector, Number offset, Number count, Number start)
        {
            this.vector = vector;

            this.offset = offset;

            this.count = count;

            this.start = start;
        }

        public Number Count => count;

        Number IVector<Number, T>.Start => start;

        private Number StartIndex => start;

        private Number EndIndex => start + count - Number.One;


        public T this[Number address]
        {
            get
            {
                VerifyAddress(address);

                return RawGet(address);
            }
        }

        private void VerifyAddress(Number address)
        {
            if (!IsValidAddress(address))
                throw new ArgumentOutOfRangeException(nameof(address));
        }

        private T RawGet(Number verifiedAddress) => this.vector[offset + (verifiedAddress - start)];

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

        public bool IsValidAddress(Number address) => address >= StartIndex && address <= EndIndex;



        IEnumerable<Number> ILookup<Number, T>.Addresses => Enumerables.GetRange(StartIndex, EndIndex);


        public IEnumerable<(Number Address, T Value)> Pairs
        {
            get
            {
                for (var i = new Number(); i < count; i++)
                    yield return (start + i, vector[offset + i]);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
                yield return (vector[offset + i]);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        bool IContainer<T>.Contains(T value)
        {
            if (value == null)
            {
                foreach (var item in this)
                {
                    if (value == null)
                        return true;
                }
            }
            else
            {
                foreach (var item in this)
                {
                    if (item.Equals(value))
                        return true;
                }
            }

            return false;
        }

        bool ILookup<Number, T>.HasAddress(Number address) => IsValidAddress(address);

        public override string ToString() => this.ToListString();
    }
}