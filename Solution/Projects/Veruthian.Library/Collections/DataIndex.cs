using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Collections
{
    public class DataIndex<T> : BaseIndex<T>
    {
        T[] items;

        public DataIndex() => items = new T[0];

        private DataIndex(T[] items) => this.items = items;

        public static DataIndex<T> New(int size)
        {
            ExceptionHelper.VerifyPositive(size, nameof(size));

            return new DataIndex<T>(new T[size]);
        }

        public static DataIndex<T> Of(T item) => new DataIndex<T>(new T[] { item });

        public static DataIndex<T> From(params T[] items) => new DataIndex<T>(items.Copy());

        public static DataIndex<T> Extract(IEnumerable<T> items) => new DataIndex<T>(items.ToArray());

        public static DataIndex<T> Extract(IEnumerable<T> items, int amount) => new DataIndex<T>(items.ToArray(amount));


        public override int Count => items.Length;

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

        protected override T RawGet(int verifiedIndex) => items[verifiedIndex];


        public static implicit operator DataIndex<T>(T item) => new DataIndex<T>(new T[] { item });

        public static DataIndex<T> operator +(DataIndex<T> items, T item) => new DataIndex<T>(items.items.Append(item));

        public static DataIndex<T> operator +(T item, DataIndex<T> items) => new DataIndex<T>(items.items.Prepend(item));


        public static DataIndex<T> operator +(DataIndex<T> left, T[] right) => new DataIndex<T>(left.items.AppendArray(right));

        public static DataIndex<T> operator +(T[] left, DataIndex<T> right) => new DataIndex<T>(right.items.PrependArray(left));


        public static DataIndex<T> operator +(DataIndex<T> left, IEnumerable<T> right) => new DataIndex<T>(left.items.AppendEnumerable(right));

        public static DataIndex<T> operator +(IEnumerable<T> left, DataIndex<T> right) => new DataIndex<T>(right.items.PrependEnumerable(left));


        public static DataIndex<T> operator +(DataIndex<T> left, IContainer<T> right) => new DataIndex<T>(left.items.AppendContainer(right));

        public static DataIndex<T> operator +(IContainer<T> left, DataIndex<T> right) => new DataIndex<T>(right.items.PrependContainer(left));


        public static DataIndex<T> operator +(DataIndex<T> left, DataIndex<T> right) => new DataIndex<T>(left.items.AppendContainer(right));


        public static DataIndex<T> operator *(DataIndex<T> items, int times) => new DataIndex<T>(items.items.Multiply(times));
    }
}