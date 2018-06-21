using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class ItemSet<T> : ItemSetBase<T>
    {        
        HashSet<T> items = new HashSet<T>();

        public sealed override int Count => items.Count;

        public sealed override void Add(T value) => items.Add(value);

        public sealed override bool Remove(T value) => items.Remove(value);

        public sealed override void Clear() => items.Clear();

        public sealed override bool Contains(T value) => items.Contains(value);

        public sealed override IEnumerable<T> Values => items;
    }
}