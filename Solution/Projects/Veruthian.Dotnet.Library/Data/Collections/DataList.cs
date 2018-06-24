using System;
using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class DataList<T> : ExpandableIndexBase<T>
    {
        protected const int minimumSize = 4;

        T[] items;

        int size = 0;
        

        public DataList() => throw new NotImplementedException();

        public DataList(int capacity) => throw new NotImplementedException();

        public DataList(params T[] items) => throw new NotImplementedException();

        public DataList(IEnumerable<T> items) => throw new NotImplementedException();

        public DataList(T item, int repeat) => throw new NotImplementedException();


        public override int Count => throw new NotImplementedException();


        protected override T RawGet(int adjustedValidIndex) => throw new NotImplementedException();

        protected override void RawSet(int adjustedValidIndex, T value) => throw new NotImplementedException();


        public override void Add(T value) => throw new NotImplementedException();

        public override void AddRange(IEnumerable<T> values) => throw new NotImplementedException();

        public override void Insert(int index, T value) => throw new NotImplementedException();

        public override void InsertRange(int index, IEnumerable<T> values) => throw new NotImplementedException();


        public override bool Remove(T value) => throw new NotImplementedException();

        public override void RemoveBy(int index) => throw new NotImplementedException();

        public override void RemoveRange(int start, int count) => throw new NotImplementedException();


        public override void Clear() => throw new NotImplementedException();
    }
}