using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class ItemArray<T> : ItemArrayBase<T>
    {
        public ItemArray() : base(new T[0]) { }

        public ItemArray(int size) : base(new T[size]) { }

        public ItemArray(params T[] items) : base(items) { }

        public ItemArray(IEnumerable<T> items) : base(items.ToArray()) { }

        public ItemArray(ILookup<int, T> items) : base(items.ToArray()) { }

        public ItemArray(T item, int repeat) : base(item.RepeatAsArray(repeat)) { }



        public sealed override int Count => items.Length;
    }
}