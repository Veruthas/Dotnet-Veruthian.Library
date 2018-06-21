using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class ItemSet<T> : ItemSetBase<T>
    {        
        HashSet<T> items = new HashSet<T>();

        public override int Count => items.Count;

        public override void Add(T value) => items.Add(value);

        public override bool Remove(T value) => items.Remove(value);

        public override void Clear() => items.Clear();

        public override bool Contains(T value) => items.Contains(value);

        public override IEnumerable<T> Values => items;
    }
}