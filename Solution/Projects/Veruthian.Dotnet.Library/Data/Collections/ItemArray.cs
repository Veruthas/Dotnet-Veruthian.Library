using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class ItemArray<T> : ISettableLookup<int, T>, IEnumerable<T>
    {
        T[] items;

        public ItemArray() => this.items = new T[0];

        public ItemArray(params T[] items) => this.items = items;

        public ItemArray(IEnumerable<T> items) => this.items = items.ToArray();

        public ItemArray(ILookup<int, T> items)
        {
            this.items = new T[items.Count];

            for (int i = 0; i < items.Count; i++)
                this.items[i] = items[i];
        }

        public ItemArray(int size) => this.items = new T[size];

        public ItemArray(T item, int repeated)
        {
            this.items = new T[repeated];

            for (int i = 0; i < items.Length; i++)
                items[i] = item;
        }


        public T this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }

        T ILookup<int, T>.this[int index] => this[index];


        public int Count => items.Length;

        public bool TryGet(int index, out T value)
        {
            if (index > 0 && index <= items.Length)
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
                foreach (var item in items)
                    yield return item;
            }
        }

        bool ILookup<int, T>.HasKey(int index) => index > 0 && index < Count;

        public IEnumerable<KeyValuePair<int, T>> Pairs => throw new System.NotImplementedException();


        public static implicit operator ItemArray<T>(T[] array) => new ItemArray<T>(array);
    }
}