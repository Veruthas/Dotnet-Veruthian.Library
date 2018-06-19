using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class ItemArray<T> : ISettableLookup<int, T>, IEnumerable<T>
    {
        T[] items;

        bool defaultable;

        protected ItemArray(T[] items, bool defaultable)
        {
            this.items = items;

            this.defaultable = defaultable;
        }


        public ItemArray() : this(new T[0], false) { }

        public ItemArray(int size) : this(new T[size], false) { }

        public ItemArray(T item, int repeated) : this(item.RepeatAsArray(), false) { }

        public ItemArray(params T[] items) : this(items, false) { }

        public ItemArray(IEnumerable<T> items) : this(items.ToArray(), false) { }

        public ItemArray(ILookup<int, T> items) : this(items.ToArray(), false) { }


        public ItemArray(bool defaultable) : this(new T[0], defaultable) { }

        public ItemArray(bool defaultable, int size) : this(new T[size], false) { }

        public ItemArray(bool defaultable, T item, int repeated) : this(item.RepeatAsArray(), defaultable) { }

        public ItemArray(bool defaultable, params T[] items) : this(items, defaultable) { }

        public ItemArray(bool defaultable, IEnumerable<T> items) : this(items.ToArray(), defaultable) { }

        public ItemArray(bool defaultable, ILookup<int, T> items) : this(items.ToArray(), defaultable) { }




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

        public int Count => items.Length;


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

        public ItemArray<T> Resize(int newSize)
        {
            T[] newItems = new T[newSize];

            System.Array.Copy(this.items, newItems, Math.Min(newSize, newItems.Length));

            return new ItemArray<T>(newItems);
        }

        public T[] InternalArray => items;

        public T[] ToArray() => (T[])items.Clone();

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

        IEnumerable<T> ILookup<int, T>.Values => Values;

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


        public static implicit operator ItemArray<T>(T[] array) => new ItemArray<T>(array);
    }
}