using System.Collections.Generic;
using Veruthian.Library.Numeric;
using Veruthian.Library.Utility.Extensions;

namespace Veruthian.Library.Collections
{
    public class DataString<T> : BaseString<T, DataString<T>>
    {
        protected override DataString<T> This => this;

        public static DataString<T> operator +(DataString<T> left, DataString<T> right)
            => left.Appended(right);

        public static DataString<T> operator +(DataString<T> left, T right)
            => left.Appended(right);

        public static DataString<T> operator +(DataString<T> left, T[] right)
            => left.Appended(right);

        public static DataString<T> operator +(DataString<T> left, IEnumerable<T> right)
            => left.Appended(right);


        public static DataString<T> operator *(DataString<T> left, Number right)
            => left.Repeated(right);

        public static DataString<T> operator ~(DataString<T> left)
            => left.Reversed();

        public static bool operator ==(DataString<T> left, DataString<T> right)
            => Equals(left, right);

        public static bool operator !=(DataString<T> left, DataString<T> right)
            => !Equals(left, right);

        public static bool operator >(DataString<T> left, DataString<T> right)
            => left.IsNotNull() && Compare(left, right) > 0;

        public static bool operator <(DataString<T> left, DataString<T> right)
            => left.IsNotNull() && Compare(left, right) < 0;

        public static bool operator >=(DataString<T> left, DataString<T> right)
            => left.IsNotNull() && Compare(left, right) >= 0;

        public static bool operator <=(DataString<T> left, DataString<T> right)
            => left.IsNotNull() && Compare(left, right) <= 0;

        
        public override bool Equals(object other) => base.Equals(other);

        public override int GetHashCode() => base.GetHashCode();
    }
}