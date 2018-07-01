using System;
using System.Collections.Generic;
using System.Linq;
using Veruthian.Dotnet.Library.Collections.Extensions;

namespace Veruthian.Dotnet.Library.Collections
{
    public class DataList<T> : ExpandableIndexBase<T>
    {
        List<T> items;

        public DataList() => this.items = new List<T>();

        public DataList(int capacity) => this.items = new List<T>(capacity);

        public DataList(params T[] items) => this.items = items.ToList();

        public DataList(IEnumerable<T> items) => this.items = items.ToList();

        public DataList(T item, int repeat) => this.items = item.RepeatAsList(repeat);


        public override int Count => items.Count;


        protected override T RawGet(int adjustedValidIndex) => items[adjustedValidIndex];

        protected override void RawSet(int adjustedValidIndex, T value) => items[adjustedValidIndex] = value;


        public override void Add(T value) => items.Add(value);

        public override void AddRange(IEnumerable<T> values) => items.AddRange(values);

        public override void Insert(int index, T value) => items.Insert(index, value);

        public override void InsertRange(int index, IEnumerable<T> values) => items.InsertRange(index, values);


        public override bool Remove(T value) => items.Remove(value);

        public override void RemoveBy(int index) => items.RemoveAt(index);

        public override void RemoveRange(int start, int count) => items.RemoveRange(start, count);


        public override void Clear() => items.Clear();
    }
}