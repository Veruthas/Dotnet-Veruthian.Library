using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Collections
{
    public class DataArray<T> : BaseMutableVector<T, DataArray<T>>
    {
        T[] items;

        public DataArray() => items = new T[0];


        public sealed override Number Count => items.Length;

        protected sealed override T RawGet(Number verifiedAddress) => items[verifiedAddress.ToCheckedSignedInt()];

        protected sealed override void RawSet(Number verifiedAddress, T value) => items[verifiedAddress.ToCheckedSignedInt()] = value;

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

        protected override void SetSize(Number size) => items = new T[size.ToCheckedInt()];

        protected override void SetData(T[] items) => this.items = items;


        public static implicit operator DataArray<T>(T item) => From(new T[] { item });

        public static DataArray<T> operator +(DataArray<T> items, T item) => From(items.items.Append(item));

        public static DataArray<T> operator +(T item, DataArray<T> items) => From(items.items.Prepend(item));


        public static DataArray<T> operator +(DataArray<T> left, T[] right) => From(left.items.AppendArray(right));

        public static DataArray<T> operator +(T[] left, DataArray<T> right) => From(right.items.PrependArray(left));


        public static DataArray<T> operator +(DataArray<T> left, IEnumerable<T> right) => From(left.items.AppendEnumerable(right));

        public static DataArray<T> operator +(IEnumerable<T> left, DataArray<T> right) => From(right.items.PrependEnumerable(left));


        public static DataArray<T> operator +(DataArray<T> left, IContainer<T> right) => From(left.items.AppendContainer(right));

        public static DataArray<T> operator +(IContainer<T> left, DataArray<T> right) => From(right.items.PrependContainer(left));


        public static DataArray<T> operator +(DataArray<T> left, DataArray<T> right) => From(left.items.AppendContainer(right));


        public static DataArray<T> operator *(DataArray<T> items, int times) => From(items.items.Multiply(times));
    }
}