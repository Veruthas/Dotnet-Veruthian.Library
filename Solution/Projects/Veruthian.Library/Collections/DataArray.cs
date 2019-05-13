using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Collections
{
    public class DataArray<T> : BaseMutableVector<T>
    {
        T[] items;

        public DataArray() => items = new T[0];

        private DataArray(T[] items) => this.items = items;


        public static DataArray<T> New(int size)
        {
            ExceptionHelper.VerifyPositive(size, nameof(size));

            return new DataArray<T>(new T[size]);
        }

        public static DataArray<T> Of(T item) => new DataArray<T>(new T[] {item});

        public static DataArray<T> From(params T[] items) => new DataArray<T>(items.Copy());

        public static DataArray<T> Extract(IEnumerable<T> items) => new DataArray<T>(items.ToArray());

        public static DataArray<T> Extract(IEnumerable<T> items, int amount) => new DataArray<T>(items.ToArray(amount));


        public sealed override int Count => items.Length;

        protected sealed override T RawGet(int verifiedAddress) => items[verifiedAddress];

        protected sealed override void RawSet(int verifiedAddress, T value) => items[verifiedAddress] = value;

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


        public static implicit operator DataArray<T>(T item) => new DataArray<T>(new T[] { item });

        public static DataArray<T> operator +(DataArray<T> items, T item) => new DataArray<T>(items.items.Append(item));

        public static DataArray<T> operator +(T item, DataArray<T> items) => new DataArray<T>(items.items.Prepend(item));


        public static DataArray<T> operator +(DataArray<T> left, T[] right) => new DataArray<T>(left.items.AppendArray(right));

        public static DataArray<T> operator +(T[] left, DataArray<T> right) => new DataArray<T>(right.items.PrependArray(left));


        public static DataArray<T> operator +(DataArray<T> left, IEnumerable<T> right) => new DataArray<T>(left.items.AppendEnumerable(right));

        public static DataArray<T> operator +(IEnumerable<T> left, DataArray<T> right) => new DataArray<T>(right.items.PrependEnumerable(left));


        public static DataArray<T> operator +(DataArray<T> left, IContainer<T> right) => new DataArray<T>(left.items.AppendContainer(right));

        public static DataArray<T> operator +(IContainer<T> left, DataArray<T> right) => new DataArray<T>(right.items.PrependContainer(left));


        public static DataArray<T> operator +(DataArray<T> left, DataArray<T> right) => new DataArray<T>(left.items.AppendContainer(right));


        public static DataArray<T> operator *(DataArray<T> items, int times) => new DataArray<T>(items.items.Multiply(times));
    }
}