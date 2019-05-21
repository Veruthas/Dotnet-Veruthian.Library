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
    public class DataVector<T> : BaseMutableVector<Number, T, DataVector<T>>, IMutableVector<T>
    {
        public static DataVector<T> Default { get; } = new DataVector<T>();


        public override Number Start => Number.Zero;

        protected override int VerifiedAddressToIndex(Number address) => (int)address;

        protected override Number OffsetStartAddress(Number offset) => offset;

        public override bool IsValidAddress(Number address) => address < Count;


        public static implicit operator DataVector<T>(T item) => From(new T[] { item });

        public static DataVector<T> operator +(DataVector<T> items, T item) => From(items.Items.Append(item));

        public static DataVector<T> operator +(T item, DataVector<T> items) => From(items.Items.Prepend(item));


        public static DataVector<T> operator +(DataVector<T> left, T[] right) => From(left.Items.AppendArray(right));

        public static DataVector<T> operator +(T[] left, DataVector<T> right) => From(right.Items.PrependArray(left));


        public static DataVector<T> operator +(DataVector<T> left, IEnumerable<T> right) => From(left.Items.AppendEnumerable(right));

        public static DataVector<T> operator +(IEnumerable<T> left, DataVector<T> right) => From(right.Items.PrependEnumerable(left));


        public static DataVector<T> operator +(DataVector<T> left, IContainer<T> right) => From(left.Items.AppendContainer(right));

        public static DataVector<T> operator +(IContainer<T> left, DataVector<T> right) => From(right.Items.PrependContainer(left));


        public static DataVector<T> operator +(DataVector<T> left, DataVector<T> right) => From(left.Items.AppendContainer(right));


        public static DataVector<T> operator *(DataVector<T> items, int times) => From(items.Items.Multiply(times));
    }
}