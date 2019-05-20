using System;
using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public class DataList<T> : BaseMutableVector<T, DataList<T>>, IResizableVector<T>
    {
        List<T> items;


        public DataList() => this.items = new List<T>();


        public sealed override Number Count => items.Count;


        protected sealed override T RawGet(Number verifiedAddress) => items[verifiedAddress.ToCheckedSignedInt()];

        protected sealed override void RawSet(Number verifiedAddress, T value) => items[verifiedAddress.ToCheckedSignedInt()] = value;

        public sealed override bool Contains(T value) => items.Contains(value);


        protected override void SetSize(Number size)
        {
            var checkedSize = size.ToCheckedSignedInt();

            this.items = new List<T>(checkedSize);

            for (var i = 0; i < checkedSize; i++) ;
        }

        protected override void SetData(T[] items)
        {
            this.items = new List<T>(items.Length);

            this.items.AddRange(items);
        }


        public void Add(T value) => items.Add(value);

        public void AddRange(IEnumerable<T> values) => items.AddRange(values);

        public void Insert(Number address, T value) => items.Insert(address.ToCheckedSignedInt(), value);

        public void InsertRange(Number address, IEnumerable<T> values) => items.InsertRange(address.ToCheckedSignedInt(), values);

        public bool Remove(T value) => items.Remove(value);

        public void RemoveBy(Number address) => items.RemoveAt(address.ToCheckedSignedInt());

        public void RemoveRange(Number address, Number count) => items.RemoveRange(address.ToCheckedSignedInt(), count.ToCheckedSignedInt());

        public void Clear() => items.Clear();



        private static DataList<T> Make(List<T> items)
        {
            var list = new DataList<T>();

            list.items = items;

            return list;
        }


        public static DataList<T> NewWith(Number capacity) => Make(new List<T>(capacity.ToCheckedSignedInt()));

        public static DataList<T> FromWith(IEnumerable<T> items, Number capacity)
        {
            var list = new List<T>(capacity.ToCheckedSignedInt());

            list.AddRange(items);

            return Make(list);
        }
    }
}