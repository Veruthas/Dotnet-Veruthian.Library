using System;
using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Collections
{
    public class DataList<T> : BaseMutableIndex<T>, IExpandableIndex<T>
    {
        List<T> items;


        public DataList() => this.items = new List<T>();

        public DataList(int capacity) => this.items = new List<T>(capacity);

        public DataList(IEnumerable<T> items) => this.items = new List<T>(items);

        public DataList(IEnumerable<T> items, int capacity)
        {
            this.items = new List<T>(capacity);

            this.items.AddRange(items);
        }


        public static DataList<T> New(int size)
        {
            var list = new DataList<T>(size);

            for (int i = 0; i < size; i++)
                list.Add(default(T));

            return list;
        }

        public static DataList<T> Of(T item)
        {
            var list = new DataList<T>();

            list.Add(item);

            return list;
        }

        public static DataList<T> From(params T[] items) => new DataList<T>(items);

        public static DataList<T> Extract(IEnumerable<T> items) => new DataList<T>(items);

        public static DataList<T> Extract(IEnumerable<T> items, int amount) => new DataList<T>(items.Extract(amount));


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