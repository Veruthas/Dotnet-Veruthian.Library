using System;
using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public class DataList<T> : BaseMutableVector<T>, IResizableVector<T>
    {
        List<T> items;


        public DataList() => this.items = new List<T>();

        public DataList(Number capacity) => this.items = new List<T>(capacity.ToCheckedSignedInt());

        public DataList(IEnumerable<T> items) => this.items = new List<T>(items);

        public DataList(IEnumerable<T> items, Number capacity)
        {
            this.items = new List<T>(capacity.ToCheckedSignedInt());

            this.items.AddRange(items);
        }


        public static DataList<T> New()
        {
            return new DataList<T>();
        }

        public static DataList<T> New(Number size)
        {
            var list = new DataList<T>(size);

            for (var i = new Number(); i < size; i++)
                list.Add(default(T));

            return list;
        }

        public static DataList<T> Of(T item)
        {
            var list = new DataList<T>();

            list.Add(item);

            return list;
        }

        public static DataList<T> From(params T[] items) => new DataList<T>(items);

        public static DataList<T> Extract(IEnumerable<T> items) => new DataList<T>(items);

        public static DataList<T> Extract(IEnumerable<T> items, Number amount) => new DataList<T>(items.Extract(amount.ToCheckedSignedInt()));

        public sealed override Number Count => items.Count;


        protected sealed override T RawGet(Number verifiedAddress) => items[verifiedAddress.ToCheckedSignedInt()];

        protected sealed override void RawSet(Number verifiedAddress, T value) => items[verifiedAddress.ToCheckedSignedInt()] = value;

        public sealed override bool Contains(T value) => items.Contains(value);


        public void Add(T value) => items.Add(value);

        public void AddRange(IEnumerable<T> values) => items.AddRange(values);

        public void Insert(Number address, T value) => items.Insert(address.ToCheckedSignedInt(), value);

        public void InsertRange(Number address, IEnumerable<T> values) => items.InsertRange(address.ToCheckedSignedInt(), values);

        public bool Remove(T value) => items.Remove(value);

        public void RemoveBy(Number address) => items.RemoveAt(address.ToCheckedSignedInt());

        public void RemoveRange(Number address, Number count) => items.RemoveRange(address.ToCheckedSignedInt(), count.ToCheckedSignedInt());

        public void Clear() => items.Clear();
    }
}