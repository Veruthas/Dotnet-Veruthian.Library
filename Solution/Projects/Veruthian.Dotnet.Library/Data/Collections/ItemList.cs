using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class ItemList<T> : IResizeableLookup<int, T>, IEnumerable<T>
    {
        List<T> items;

        public ItemList() => this.items = new List<T>();

        public ItemList(params T[] items) => this.items = items.ToList();

        public ItemList(IEnumerable<T> items) => this.items = items.ToList();

        public ItemList(ILookup<int, T> items)
        {
            this.items = new List<T>(items.Count);

            for (int i = 0; i < items.Count; i++)
                this.items.Add(items[i]);
        }

        public ItemList(int capacity) => this.items = new List<T>(capacity);

        public ItemList(T item, int repeated)
        {
            this.items = new List<T>(repeated);

            for (int i = 0; i < repeated; i++)
                this.items.Add(item);
        }


        public T this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }

        T ILookup<int, T>.this[int index] => items[index];

        public int Count => items.Count;


        public void Clear() => items.Clear();

        public void Insert(int index, T value) => items.Insert(index, value);

        public void Remove(T value) => items.Remove(value);

        public void RemoveBy(int index) => items.RemoveAt(index);

        public bool TryGet(int index, out T value)
        {
            if (index > 0 && index < Count)
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


        public List<T> InternalList() => items;

        public List<T> ToList() => new List<T>(items);


        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)items).GetEnumerator();
        }

        IEnumerable<int> ILookup<int, T>.Keys
        {
            get
            {
                for (int i = 0; i < Count; i++)
                    yield return i;
            }
        }

        IEnumerable<T> ILookup<int, T>.Values => items;

        public IEnumerable<KeyValuePair<int, T>> Pairs
        {
            get
            {
                for (int i = 0; i < Count; i++)
                    yield return new KeyValuePair<int, T>(i, items[i]);

            }
        }

        public static implicit operator ItemList<T>(List<T> items) => new ItemList<T>(items);
    }
}