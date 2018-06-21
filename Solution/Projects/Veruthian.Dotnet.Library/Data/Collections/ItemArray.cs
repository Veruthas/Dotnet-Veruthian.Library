using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class ItemArray<T> : ItemMutableArrayBase<T>
    {
        T[] items;


        public ItemArray() => this.items = new T[0];

        public ItemArray(int size) => this.items = new T[size];

        public ItemArray(params T[] items) => this.items = items;

        public ItemArray(IEnumerable<T> items) => this.items = items.ToArray();


        public sealed override int Count => items.Length;


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

            return false;
        }


        public sealed override bool Contains(T value)
        {
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];

                if (item == null)
                {
                    if (value == null)
                    {
                        return true;
                    }
                }
                else if (item.Equals(value))
                {
                    return true;
                }
            }

            return false;
        }


        protected sealed override IEnumerable<int> GetKeys()
        {
            for (int i = 0; i < Count; i++)
                yield return i;
        }

        protected sealed override IEnumerable<T> GetValues()
        {
            for (int i = 0; i < Count; i++)
                yield return items[i];
        }

        protected sealed override IEnumerable<KeyValuePair<int, T>> GetPairs()
        {
            for (int i = 0; i < Count; i++)
                yield return new KeyValuePair<int, T>(i, items[i]);
        }
    }
}