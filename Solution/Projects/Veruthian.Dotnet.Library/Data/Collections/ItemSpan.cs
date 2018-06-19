using System;
using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public struct ItemSpan<T> : ILookup<int, T>, IEnumerable<T>
    {
        ILookup<int, T> lookup;

        int start;

        int count;


        public T this[int index]
        {
            get
            {
                VerifyKey(index);

                return lookup[start + index];
            }
        }

        public bool IsDefaultable => lookup.IsDefaultable;
        
        public int Count => count;

        IEnumerable<int> ILookup<int, T>.Keys
        {
            get
            {
                for (int i = 0; i < count; i++)
                    yield return i;
            }
        }

        IEnumerable<T> ILookup<int, T>.Values => Values;

        private IEnumerable<T> Values 
        {
            get
            {
                for (int i = start; i < start + count; i++)
                    yield return lookup[i];
            }
        }

        public IEnumerable<KeyValuePair<int, T>> Pairs
        {
            get
            {
                for (int i = start; i < start + count; i++)
                    yield return new KeyValuePair<int, T>(i, lookup[start + i]);
            }
        }

        public bool HasKey(int index) => index >= 0 && index < Count;

        private void VerifyKey(int index)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException();
        }

        public bool TryGet(int index, out T value)
        {
            if (HasKey(index))
            {
                value = lookup[start + index];
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public IEnumerator<T> GetEnumerator() => Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();
    }
}