using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Collections
{
    public  class OrderedListBase<T> : OrderedIndexBase<T>
    {
        protected List<T> items;

        protected readonly IComparer<T> comparer;

        protected OrderedListBase(List<T> items, IComparer<T> comparer)
        {
            this.items = items;

            this.comparer = comparer ?? Comparer<T>.Default;
        }

        public override int Count => items.Count;

        protected bool CanDuplicate { get; }

        public override void Add(T value)
        {
            // Its sorted, binary search?
            throw new NotImplementedException();
        }

        public override bool Remove(T value) => items.Remove(value);

        public override void RemoveBy(int index) => items.RemoveAt(index);

        public override void Clear() => items.Clear();

        protected override T RawGet(int adjustedValidIndex) => items[adjustedValidIndex];
    }
}