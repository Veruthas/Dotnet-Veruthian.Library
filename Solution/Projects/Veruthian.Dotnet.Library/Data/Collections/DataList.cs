using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class DataList<T> : ExpandableIndexBase<T>
    {
        List<T> items;

        public DataList() => this.items = new List<T>();


        public DataList(int capacity) => this.items = new List<T>(capacity);

        public DataList(params T[] items) => this.items = items.ToList();

        public DataList(IEnumerable<T> items) => this.items = items.ToList();

        public DataList(T item, int repeat) => this.items = item.RepeatAsList(repeat);


        public DataList(int startIndex, int capacity) : base(startIndex) => this.items = new List<T>(capacity);

        public DataList(int startIndex, params T[] items) : base(startIndex) => this.items = items.ToList();

        public DataList(int startIndex, IEnumerable<T> items) : base(startIndex) => this.items = items.ToList();

        public DataList(int startIndex, T item, int repeat) : base(startIndex) => this.items = item.RepeatAsList(repeat);


        public override int Count => items.Count;


        protected override T RawGet(int adjustedValidIndex) => items[adjustedValidIndex];

        protected override void RawSet(int adjustedValidIndex, T value) => items[adjustedValidIndex] = value;


        public override void Add(T value) => items.Add(value);

        void AddRange(IEnumerable<T> values) => items.AddRange(values);

        public override void Insert(int index, T value) => items.Add(value);

        void InsertRange(int index, IEnumerable<T> values) => items.InsertRange(index - StartIndex, values);


        public override bool Remove(T value) => items.Remove(value);

        public override void RemoveBy(int index) => items.RemoveAt(index - StartIndex);

        void RemoveRange(int start, int count) => items.RemoveRange(start - StartIndex, count);


        public override void Clear() => items.Clear();
    }
}