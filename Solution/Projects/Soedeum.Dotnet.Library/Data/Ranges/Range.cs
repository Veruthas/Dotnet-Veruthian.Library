using System;
using System.Collections;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Data.Ranges
{
    public struct Range<T> : IEquatable<Range<T>>, IComparable<Range<T>>, IComparable<T>, IEnumerable<T>
        where T : IOrderable<T>, new()
    {
        static T main = new T();

        readonly T low, high;

        readonly int hashcode;

        public Range(T value)
        {
            if (value == null)
                throw new ArgumentOutOfRangeException("value");

            this.low = this.high = value;

            hashcode = HashCodes.Default.Combine(this.low, this.high);
        }

        public Range(T a, T b)
        {
            if (a == null)
                throw new ArgumentOutOfRangeException("a");
            if (b == null)
                throw new ArgumentOutOfRangeException("b");

            int comparison = a.CompareTo(b);

            if (comparison > 0)
            {
                this.low = b;

                this.high = a;
            }
            else
            {
                this.low = a;

                this.high = b;
            }

            hashcode = HashCodes.Default.Combine(this.low, this.high);
        }

        public T Low => low;

        public T High => high;


        public bool IsComplete => low.Equals(main.MinValue) && high.Equals(main.MaxValue);

        public bool IsSingle => low.Equals(high);


        // Comparison
        public bool Equals(Range<T> other) => low.Equals(other.low) && high.Equals(other.high);

        public override bool Equals(object other) => (other is Range<T>) ? this.Equals((Range<T>)other) : false;

        public static bool operator ==(Range<T> left, Range<T> right) => left.Equals(right);

        public static bool operator !=(Range<T> left, Range<T> right) => !left.Equals(right);


        public int CompareTo(Range<T> other)
        {
            int comparison = this.low.CompareTo(other.low);

            if (comparison < 0)
                return -1;
            else if (comparison > 0)
                return 1;
            else
                return this.high.CompareTo(other.high);
        }

        public int CompareTo(T other)
        {
            int comparison = this.low.CompareTo(other);

            if (comparison < 0)
                return -1;
            else if (comparison > 0)
                return 1;
            else
                return this.high.CompareTo(other);
        }

        public static bool operator <(Range<T> left, Range<T> right) => left.CompareTo(right) < 0;

        public static bool operator >(Range<T> left, Range<T> right) => left.CompareTo(right) > 0;

        public static bool operator <=(Range<T> left, Range<T> right) => !(left > right);

        public static bool operator >=(Range<T> left, Range<T> right) => !(left < right);


        public override int GetHashCode() => hashcode;


        public override string ToString()
        {
            if (IsSingle)
                return low.ToString();
            else
                return '(' + low.ToString() + " to " + high.ToString() + ')';
        }

        public string ToString(Func<T, string> toString)
        {
            if (IsSingle)
                return toString(low);
            else
                return '(' + toString(low) + " to " + toString(high) + ')';
        }


        public IEnumerator<T> GetEnumerator()
        {
            var current = low;

            while (current.CompareTo(high) <= 0)
            {
                yield return current;
                current = current.Next();
            }            
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}