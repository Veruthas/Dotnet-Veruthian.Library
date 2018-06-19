using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class ItemArray<T> : ItemArrayBase<T>
    {
        public ItemArray() : base(new T[0], false) { }

        public ItemArray(int size) : base(new T[size], false) { }

        public ItemArray(params T[] items) : base(items, false) { }

        public ItemArray(IEnumerable<T> items) : base(items.ToArray(), false) { }

        public ItemArray(ILookup<int, T> items) : base(items.ToArray(), false) { }

        public ItemArray(T item, int repeat) : base(item.RepeatAsArray(repeat), false) { }


        public ItemArray(bool defaultable) : base(new T[0], defaultable) { }

        public ItemArray(bool defaultable, int size) : base(new T[size], defaultable) { }

        public ItemArray(bool defaultable, params T[] items) : base(items, defaultable) { }

        public ItemArray(bool defaultable, IEnumerable<T> items) : base(items.ToArray(), defaultable) { }

        public ItemArray(bool defaultable, ILookup<int, T> items) : base(items.ToArray(), defaultable) { }

        public ItemArray(bool defaultable, T item, int repeat) : base(item.RepeatAsArray(repeat), defaultable) { }

    }
}