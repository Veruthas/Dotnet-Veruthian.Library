using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class ItemList<T> : ItemArrayBase<T>, IResizeableLookup<int, T>
    {
        const int smallest = 4;

        protected int count;


        public ItemList() : base(new T[smallest], false) { }

        public ItemList(int capacity) : base(new T[capacity], false) { }

        public ItemList(params T[] items) : base(items, false) { }

        public ItemList(IEnumerable<T> items) : base(items.ToArray(), false) { }

        public ItemList(ILookup<int, T> items) : base(items.ToArray(), false) { }

        public ItemList(T item, int repeat) : base(item.RepeatAsArray(repeat), false) { }


        public ItemList(bool defaultable) : base(new T[0], defaultable) { }

        public ItemList(bool defaultable, int capacity) : base(new T[capacity], defaultable) { }

        public ItemList(bool defaultable, params T[] items) : base(items, defaultable) { }

        public ItemList(bool defaultable, IEnumerable<T> items) : base(items.ToArray(), defaultable) { }

        public ItemList(bool defaultable, ILookup<int, T> items) : base(items.ToArray(), defaultable) { }

        public ItemList(bool defaultable, T item, int repeat) : base(item.RepeatAsArray(repeat), defaultable) { }


        public sealed override int Count => count;

        public void Add(T value)
        {
            if (count == items.Length)
                Resize();

            items[count++] = value;
        }


        public void Insert(int index, T value)
        {
            VerifyIndex(index);

            if (count == items.Length)
                Resize();

            if (index == count)
            {
                items[count++] = value;
            }
            else
            {
                Array.Copy(items, index, items, index + 1, count - index);

                items[index] = value;

                count++;
            }
        }

        public void Remove(int index)
        {
            VerifyIndex(index);

            if (index == count - 1)
            {
                items[index] = default(T);
                count--;
            }
            else
            {
                Array.Copy(items, index + 1, items, index, count - index - 1);

                items[count--] = default(T);
            }
        }

        private void VerifyIndex(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();
        }

        // Enforce power of two?
        private void Resize()
        {
            int size = smallest;

            while (size <= count)
                size <<= 1;

            T[] newItems = new T[size];

            Array.Copy(this.items, newItems, count);

            this.items = newItems;
        }

        public void Clear()
        {
            Array.Clear(items, 0, items.Length);

            count = 0;
        }
    }
}