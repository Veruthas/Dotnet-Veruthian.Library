using System.Collections.Generic;
using System.Linq;
using Veruthian.Dotnet.Library.Collections.Extensions;

namespace Veruthian.Dotnet.Library.Collections
{
    public class DataArray<T> : MutableIndexBase<T>
    {
        T[] items;


        public DataArray() => this.items = new T[0];


        public DataArray(int size) => this.items = new T[size];

        public DataArray(params T[] items) => this.items = items;

        public DataArray(IEnumerable<T> items) => this.items = items.ToArray();
        
        public DataArray(T item, int repeat) => this.items = item.RepeatAsArray(repeat);


        public override int Count => items.Length;


        protected override T RawGet(int adjustedValidIndex) => items[adjustedValidIndex];

        protected override void RawSet(int adjustedValidIndex, T value) => items[adjustedValidIndex] = value;
    }
}