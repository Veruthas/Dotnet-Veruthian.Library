using System;
using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Library.Collections
{
    public class DataList<T> : BaseMutableIndex<T>, IExpandableIndex<T>
    {
        List<T> items;


        public DataList() : this(0) { }

        public DataList(params T[] items) => this.items = new List<T>(items);

        public DataList(IEnumerable<T> items) => this.items = items.ToList();

        public DataList(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity", capacity, "Capacity cannot be negative");

            this.items = new List<T>(capacity);
        }

        public DataList(int size, IEnumerable<T> items) : this(size)
        {
            this.items.AddRange(items);
        }


        public sealed override int Count => items.Count;


        protected sealed override T RawGet(int verifiedIndex) => items[verifiedIndex];

        protected sealed override void RawSet(int verifiedIndex, T value) => items[verifiedIndex] = value;

        public sealed override bool Contains(T value) => items.Contains(value);


        public void Add(T value) => items.Add(value);

        public void AddRange(IEnumerable<T> values) => items.AddRange(values);

        public void Insert(int index, T value) => items.Insert(index, value);

        public void InsertRange(int index, IEnumerable<T> values) => items.InsertRange(index, values);

        public bool Remove(T value) => items.Remove(value);

        public void RemoveBy(int index) => items.RemoveAt(index);

        public void RemoveRange(int index, int count) => items.RemoveRange(index, count);

        public void Clear() => items.Clear();
    }
}