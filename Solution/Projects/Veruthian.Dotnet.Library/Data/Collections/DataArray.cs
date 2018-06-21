using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class DataArray<T> : MutableIndexBase<T>
    {
        T[] items;


        public DataArray() => this.items = new T[0];


        public DataArray(int size) => this.items = new T[size];

        public DataArray(params T[] items) => this.items = items;

        public DataArray(IEnumerable<T> items) => this.items = items.ToArray();
        
        public DataArray(T item, int repeat) => this.items = item.RepeatAsArray(repeat);


        public DataArray(int startIndex, int size) : base(startIndex) => this.items = new T[size];

        public DataArray(int startIndex, params T[] items) : base(startIndex) => this.items = items;

        public DataArray(int startIndex, IEnumerable<T> items) : base(startIndex) => this.items = items.ToArray();

        public DataArray(int startIndex, T item, int repeat) : base(startIndex) => this.items = item.RepeatAsArray(repeat);


        public override int Count => items.Length;


        protected override T RawGet(int adjustedValidIndex) => items[adjustedValidIndex];

        protected override void RawSet(int adjustedValidIndex, T value) => items[adjustedValidIndex] = value;
    }
}