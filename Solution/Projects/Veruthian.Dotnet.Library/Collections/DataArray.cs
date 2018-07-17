using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Veruthian.Dotnet.Library.Collections
{
    public sealed class DataArray<T> : BaseMutableIndex<T>
    {
        T[] items;

        public DataArray(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", count, "Count cannot be negative");

            this.items = new T[count];
        }

        public DataArray(params T[] items) => this.items = (T[])items.Clone();

        public DataArray(IEnumerable<T> items) => this.items = items.ToArray();


        public override int Count => items.Length;

        protected override T RawGet(int verifiedIndex) => items[verifiedIndex];

        protected override void RawSet(int verifiedIndex, T value) => items[verifiedIndex] = value;

        public override bool Contains(T value)
        {
            if (value == null)
            {
                foreach (var item in items)
                {
                    if (item == null)
                        return true;
                }
            }
            else
            {
                foreach (var item in items)
                {
                    if (item.Equals(value))
                        return true;
                }
            }

            return false;
        }
    }
}