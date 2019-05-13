using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Collections
{
    public abstract class BaseIndex<T> : IIndex<T>, IEnumerable<T>
    {

        public int Start => 0;

        public abstract int Count { get; }



        public T this[int address]
        {
            get
            {
                VerifyIndex(address);

                return RawGet(address);
            }
        }

        protected abstract T RawGet(int verifiedIndex);

        public bool TryGet(int address, out T value)
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


        protected void VerifyIndex(int address)
        {
            if (!IsValidAddress(address))
                throw new ArgumentOutOfRangeException(nameof(address));
        }

        protected bool IsValidAddress(int address) => (uint)address < Count;

        bool ILookup<int, T>.HasAddress(int address) => IsValidAddress(address);


        IEnumerable<int> ILookup<int, T>.Addresses => Enumerables.GetRange(0, Count - 1);

        public IEnumerable<(int, T)> Pairs
        {
            get
            {
                for (int i = 0; i < Count; i++)
                    yield return (i, RawGet(i));
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return RawGet(i);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        public override string ToString() => this.ToListString();
    }
}