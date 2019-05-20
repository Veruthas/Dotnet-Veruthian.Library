using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Collections
{
    public class DataVector<T> : BaseVector<T>
    {
        T[] items;

        public DataVector() => items = new T[0];

        private DataVector(T[] items) => this.items = items;

        public static DataVector<T> New(int size)
        {
            ExceptionHelper.VerifyPositive(size, nameof(size));

            return new DataVector<T>(new T[size]);
        }

        public static DataVector<T> Of(T item) => new DataVector<T>(new T[] { item });

        public static DataVector<T> From(params T[] items) => new DataVector<T>(items.Copy());

        public static DataVector<T> Extract(IEnumerable<T> items) => new DataVector<T>(items.ToArray());

        public static DataVector<T> Extract(IEnumerable<T> items, Number amount) => new DataVector<T>(items.ToArray(amount.ToCheckedSignedInt()));

        public override Number Count => items.Length;

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

        protected override T RawGet(Number verifiedAddress) => items[verifiedAddress.ToCheckedSignedInt()];


        public static implicit operator DataVector<T>(T item) => new DataVector<T>(new T[] { item });

        public static DataVector<T> operator +(DataVector<T> items, T item) => new DataVector<T>(items.items.Append(item));

        public static DataVector<T> operator +(T item, DataVector<T> items) => new DataVector<T>(items.items.Prepend(item));


        public static DataVector<T> operator +(DataVector<T> left, T[] right) => new DataVector<T>(left.items.AppendArray(right));

        public static DataVector<T> operator +(T[] left, DataVector<T> right) => new DataVector<T>(right.items.PrependArray(left));


        public static DataVector<T> operator +(DataVector<T> left, IEnumerable<T> right) => new DataVector<T>(left.items.AppendEnumerable(right));

        public static DataVector<T> operator +(IEnumerable<T> left, DataVector<T> right) => new DataVector<T>(right.items.PrependEnumerable(left));


        public static DataVector<T> operator +(DataVector<T> left, IContainer<T> right) => new DataVector<T>(left.items.AppendContainer(right));

        public static DataVector<T> operator +(IContainer<T> left, DataVector<T> right) => new DataVector<T>(right.items.PrependContainer(left));


        public static DataVector<T> operator +(DataVector<T> left, DataVector<T> right) => new DataVector<T>(left.items.AppendContainer(right));


        public static DataVector<T> operator *(DataVector<T> items, int times) => new DataVector<T>(items.items.Multiply(times));
    }
}