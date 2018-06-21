using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class ItemList<T> : ItemListBase<T>
    {
        List<T> items;

        public ItemList() => this.items = new List<T>();

        public ItemList(int capacity) => this.items = new List<T>(capacity);

        public ItemList(params T[] items) => this.items = new List<T>(items);

        public ItemList(IEnumerable<T> items) => this.items = new List<T>(items);


        public sealed override int Count => items.Count;

        public sealed override bool TryGet(int index, out T value)
        {
            if (IsValidIndex(index))
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

        public sealed override bool TrySet(int index, T value)
        {
            if (IsValidIndex(index))
            {
                items[index] = value;

                return true;
            }
            else
            {
                return false;
            }
        }


        public sealed override bool Contains(T value) => items.Contains(value);


        public sealed override void Add(T value) => items.Add(value);

        public sealed override void Insert(int index, T value) => items.Insert(index, value);

        public sealed override bool Remove(T value) => items.Remove(value);

        public sealed override void RemoveBy(int index) => items.RemoveAt(index);

        public sealed override void Clear() => items.Clear();

        protected sealed override IEnumerable<int> GetKeys()
        {
            for (int i = 0; i < items.Count; i++)
                yield return i;
        }

        protected sealed override IEnumerable<T> GetValues()
        {
            for (int i = 0; i < items.Count; i++)
                yield return items[i];
        }

        protected sealed override IEnumerable<KeyValuePair<int, T>> GetPairs()
        {
            for (int i = 0; i < items.Count; i++)
                yield return new KeyValuePair<int, T>(i, items[i]);
        }
    }
}