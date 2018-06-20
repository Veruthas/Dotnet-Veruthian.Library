using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public abstract class ItemSetBase<T> : IExpandableContainer<T>
    {
        public abstract int Count { get; }

        public abstract bool Contains(T value);

        public abstract IEnumerable<T> Values { get; }

        public abstract void Add(T value);

        public abstract bool Remove(T value);

        public abstract void Clear();

    }
}