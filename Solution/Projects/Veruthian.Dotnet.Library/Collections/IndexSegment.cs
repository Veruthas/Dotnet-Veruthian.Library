using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Veruthian.Dotnet.Library.Collections.Extensions;

namespace Veruthian.Dotnet.Library.Collections
{
    public struct IndexSegment<T> : IIndex<T>, IEnumerable<T>
    {
        IIndex<T> index;

        int offset;

        int count;

        int start;



        public IndexSegment(IIndex<T> index)
        {
            this.index = index;

            this.offset = 0;

            this.count = index.Count;

            this.start = 0;
        }

        public IndexSegment(IIndex<T> index, int offset)
        {
            this.index = index;

            this.offset = offset;

            this.count = index.Count - offset;

            this.start = 0;
        }

        public IndexSegment(IIndex<T> index, int offset, int count)
        {
            this.index = index;

            this.offset = offset;

            this.count = count;

            this.start = 0;
        }

        public IndexSegment(IIndex<T> index, int offset, int count, int start)
        {
            this.index = index;

            this.offset = offset;

            this.count = count;

            this.start = start;
        }


        public int Count => count;

        int IIndex<int, T>.Start => start;

        private int StartIndex => start;

        private int EndIndex => start + count - 1;


        public T this[int index]
        {
            get
            {
                VerifyIndex(index);

                return RawGet(index);
            }
        }

        private void VerifyIndex(int index)
        {
            if (!IsValidIndex(index))
                throw new IndexOutOfRangeException();
        }

        private T RawGet(int verifiedIndex) => this.index[offset + (verifiedIndex - start)];

        public bool TryGet(int index, out T value)
        {
            if (IsValidIndex(index))
            {
                value = RawGet(index);

                return true;
            }
            else
            {
                value = default(T);

                return false;
            }
        }

        public bool IsValidIndex(int index) => index >= StartIndex && index <= EndIndex;



        IEnumerable<int> ILookup<int, T>.Keys => Enumerables.GetRange(StartIndex, EndIndex);


        IEnumerable<T> IContainer<T>.Values
        {
            get
            {
                for (int i = 0; i < count; i++)
                    yield return (index[offset + i]);
            }
        }

        public IEnumerable<(int, T)> Pairs
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

        bool ILookup<int, T>.HasKey(int key) => IsValidIndex(key);

        public override string ToString() => this.ToListString();
    }
}