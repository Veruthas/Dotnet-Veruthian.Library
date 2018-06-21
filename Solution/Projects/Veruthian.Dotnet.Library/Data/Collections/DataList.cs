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


        public override int Count => throw new System.NotImplementedException();

        protected override T RawGet(int adjustedValidIndex)
        {
            throw new System.NotImplementedException();
        }

        protected override void RawSet(int adjustedValidIndex, T value)
        {
            throw new System.NotImplementedException();
        }

        public override void Add(T value)
        {
            throw new System.NotImplementedException();
        }

        public override void Clear()
        {
            throw new System.NotImplementedException();
        }

        public override void Insert(int key, T value)
        {
            throw new System.NotImplementedException();
        }

        public override bool Remove(T value)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveBy(int key)
        {
            throw new System.NotImplementedException();
        }
    }
}