using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Collections
{
    public class DataIndex<T> : BaseIndex<T>
    {
        T[] items;


        public DataIndex() => this.items = new T[0];

        public DataIndex(IEnumerable<T> items) => this.items = items.ToArray();

        private DataIndex(T[] items) => this.items = items;


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


        public static DataIndex<T> operator +(DataIndex<T> items, T item) => new DataIndex<T>(items.items.Append(item));

        public static DataIndex<T> operator +(T item, DataIndex<T> items) => new DataIndex<T>(items.items.Prepend(item));

        public static DataIndex<T> operator +(DataIndex<T> left, DataIndex<T> right) => new DataIndex<T>(left.items.AppendContainer(right));
    }
}