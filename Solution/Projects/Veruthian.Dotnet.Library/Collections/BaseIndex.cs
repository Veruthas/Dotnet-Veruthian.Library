using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Veruthian.Dotnet.Library.Collections.Extensions;

namespace Veruthian.Dotnet.Library.Collections
{
    public abstract class BaseIndex<T> : IIndex<T>, IEnumerable<T>
    {

        public int Start => 0;

        public abstract int Count { get; }



        public T this[int index]
        {
            get
            {
                VerifyIndex(index);

                return RawGet(index);
            }
        }

        protected abstract T RawGet(int verifiedIndex);

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

        public abstract bool Contains(T value);


        protected void VerifyIndex(int index)
        {
            if (!IsValidIndex(index))
                throw new IndexOutOfRangeException();
        }

        protected bool IsValidIndex(int index) => (uint)index < Count;

        bool ILookup<int, T>.HasKey(int index) => IsValidIndex(index);


        IEnumerable<int> ILookup<int, T>.Keys => Enumerables.GetRange(0, Count - 1);

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