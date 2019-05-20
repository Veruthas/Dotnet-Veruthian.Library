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
    public class DataVector<T> : BaseMutableVector<T, DataVector<T>>
    {
        public static DataVector<T> Default { get; } = new DataVector<T>();


        public static implicit operator DataVector<T>(T item) => From(new T[] { item });

        public static DataVector<T> operator +(DataVector<T> items, T item) => From(items.items.Append(item));

        public static DataVector<T> operator +(T item, DataVector<T> items) => From(items.items.Prepend(item));


        public static DataVector<T> operator +(DataVector<T> left, T[] right) => From(left.items.AppendArray(right));

        public static DataVector<T> operator +(T[] left, DataVector<T> right) => From(right.items.PrependArray(left));


        public static DataVector<T> operator +(DataVector<T> left, IEnumerable<T> right) => From(left.items.AppendEnumerable(right));

        public static DataVector<T> operator +(IEnumerable<T> left, DataVector<T> right) => From(right.items.PrependEnumerable(left));


        public static DataVector<T> operator +(DataVector<T> left, IContainer<T> right) => From(left.items.AppendContainer(right));

        public static DataVector<T> operator +(IContainer<T> left, DataVector<T> right) => From(right.items.PrependContainer(left));


        public static DataVector<T> operator +(DataVector<T> left, DataVector<T> right) => From(left.items.AppendContainer(right));


        public static DataVector<T> operator *(DataVector<T> items, int times) => From(items.items.Multiply(times));
    }
}