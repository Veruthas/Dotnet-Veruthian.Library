using System;
using System.Collections;
using System.Collections.Generic;
using Veruthian.Dotnet.Library.Numeric;

namespace Veruthian.Dotnet.Library.Collections
{
    public abstract class BaseIndex<T> : IIndex<T>, IEnumerable<T>
    {
        
        public T this[int index]
        {
            get => IsValidIndex(index) ? RawGet(index) : throw new IndexOutOfRangeException();
        }

        IEnumerable<int> ILookup<int, T>.Keys
        {
            get
            {
                return NumericUtility.GetRange(0, Count - 1);
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
                for (int i = 0, index = 0; i < Count; i++, index++)
                {
                    var item = RawGet(i);

                    yield return new KeyValuePair<int, T>(index, item);
                }
            }
        }

        public IEnumerator<T> GetEnumerator() => Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();


        public bool IsValidIndex(int index) => (uint)index < Count;

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
                value = RawGet(index);

                return true;
            }
            else
            {
                value = default(T);

                return false;
            }
        }


        public abstract int Count { get; }

        protected abstract T RawGet(int validIndex);
    }
}