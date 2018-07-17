using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Veruthian.Dotnet.Library.Collections
{
    public class DataArray<T> : BaseMutableIndex<T>
    {
        T[] items;

        public DataArray() : this(0) { }

        public DataArray(params T[] items) => this.items = (T[])items.Clone();

        public DataArray(IEnumerable<T> items) => this.items = items.ToArray();

        public DataArray(int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException("size", size, "Size cannot be negative");

            this.items = new T[size];
        }
        
        public DataArray(int size, IEnumerable<T> items) : this(size)
        {
            var i = 0;

            foreach (var item in items)
            {
                if (i < size)
                    this.items[i++] = item;
                else
                    break;
            }
        }


        public sealed override int Count => items.Length;

        protected sealed override T RawGet(int verifiedIndex) => items[verifiedIndex];

        protected sealed override void RawSet(int verifiedIndex, T value) => items[verifiedIndex] = value;

        public sealed override bool Contains(T value)
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