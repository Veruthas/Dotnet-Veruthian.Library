using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public abstract class ItemArrayBase<T> : ISettableLookup<int, T>, IEnumerable<T>
    {
        protected T[] items;

        protected bool defaultable;

        protected ItemArrayBase(T[] items, bool defaultable)
        {
            this.items = items;

            this.defaultable = defaultable;
        }


        public T this[int index]
        {
            get => HasKey(index) ? items[index] : defaultable ? default(T) : throw new IndexOutOfRangeException();
            set
            {
                if (HasKey(index))
                    items[index] = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        T ILookup<int, T>.this[int index] => this[index];

        public bool IsDefaultable => defaultable;

        public abstract int Count { get; }

        public bool HasKey(int index) => index >= 0 && index < Count;

        public bool TryGet(int index, out T value)
        {
            if (HasKey(index))
            {
                value = items[index];

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

        IEnumerable<int> ILookup<int, T>.Keys
        {
            get
            {
                for (int i = 0; i < Count; i++)
                    yield return i;
            }
        }

        IEnumerable<T> ILookup<int, T>.Values
        {
            get => Values;
        }

        private IEnumerable<T> Values
        {
            get
            {
                for (int i = 0; i < Count; i++)
                    yield return items[i];
            }
        }

        public IEnumerable<KeyValuePair<int, T>> Pairs
        {
            get
            {
                for (int i = 0; i < Count; i++)
                    yield return new KeyValuePair<int, T>(i, items[i]);
            }
        }
    }
}