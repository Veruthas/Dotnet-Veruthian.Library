using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Collections
{
    public struct IndexSegment<T> : IVector<T>, IEnumerable<T>
    {
        IVector<T> index;

        int offset;

        int count;

        int start;



        public IndexSegment(IVector<T> index)
        {
            this.index = index;

            this.offset = 0;

            this.count = index.Count;

            this.start = 0;
        }

        public IndexSegment(IVector<T> index, int offset)
        {
            this.index = index;

            this.offset = offset;

            this.count = index.Count - offset;

            this.start = 0;
        }

        public IndexSegment(IVector<T> index, int offset, int count)
        {
            this.index = index;

            this.offset = offset;

            this.count = count;

            this.start = 0;
        }

        public IndexSegment(IVector<T> index, int offset, int count, int start)
        {
            this.index = index;

            this.offset = offset;

            this.count = count;

            this.start = start;
        }


        public int Count => count;

        int IVector<int, T>.Start => start;

        private int StartIndex => start;

        private int EndIndex => start + count - 1;


        public T this[int address]
        {
            get
            {
                VerifyAddress(address);

                return RawGet(address);
            }
        }

        private void VerifyAddress(int address)
        {
            if (!IsValidAddress(address))
                throw new ArgumentOutOfRangeException(nameof(address));
        }

        private T RawGet(int verifiedAddress) => this.index[offset + (verifiedAddress - start)];

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

        public bool IsValidAddress(int address) => address >= StartIndex && address <= EndIndex;



        IEnumerable<int> ILookup<int, T>.Addresses => Enumerables.GetRange(StartIndex, EndIndex);


        public IEnumerable<(int Address, T Value)> Pairs
        {
            get
            {
                for (int i = 0; i < count; i++)
                    yield return (start + i, index[offset + i]);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
                yield return (index[offset + i]);
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

        bool ILookup<int, T>.HasAddress(int address) => IsValidAddress(address);

        public override string ToString() => this.ToListString();
    }
}