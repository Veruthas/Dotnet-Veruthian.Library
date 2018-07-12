using System;
using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Collections
{
    public abstract class BaseIndex<T> : IIndex<T>, IEnumerable<T>
    {
        protected int startIndex;


        protected BaseIndex(int startIndex = 0) => this.startIndex = startIndex;


        public T this[int index]
        {
            get => TryGet(index, out var value) ? value : throw new IndexOutOfRangeException();
        }

        public int StartIndex
        {
            get => startIndex;
            set => this.startIndex = value;
        }

        public int EndIndex => startIndex + Count - 1;

        IEnumerable<int> ILookup<int, T>.Keys
        {
            get
            {
                for (int i = StartIndex; i < StartIndex + Count; i++)
                    yield return i;
            }
        }

        public IEnumerable<T> Values
        {
            get
            {
                for (int i = 0; i < Count; i++)
                    yield return RawGet(i);
            }
        }

        public IEnumerable<KeyValuePair<int, T>> Pairs
        {
            get
            {
                for (int i = 0, index = StartIndex; i < Count; i++, index++)
                {
                    var item = RawGet(i);

                    yield return new KeyValuePair<int, T>(index, item);
                }
            }
        }

        public IEnumerator<T> GetEnumerator() => Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();


        public bool IsValidIndex(int index) => index >= StartIndex && index < StartIndex + Count;

        bool ILookup<int, T>.HasKey(int index) => IsValidIndex(index);


        public bool Contains(T value)
        {
            for (int i = 0; i < Count; i++)
            {
                var item = RawGet(i);

                if (item == null)
                {
                    if (value == null)
                        return true;
                }
                else
                {
                    if (item.Equals(value))
                        return true;
                }
            }

            return false;
        }


        public bool TryGet(int index, out T value)
        {
            if (IsValidIndex(index))
            {
                value = RawGet(index + StartIndex);

                return true;
            }
            else
            {
                value = default(T);

                return false;
            }
        }


        public abstract int Count { get; }

        protected abstract T RawGet(int adjustedValidIndex);
    }
}