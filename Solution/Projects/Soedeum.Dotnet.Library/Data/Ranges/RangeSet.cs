using System;
using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Data.Enumeration;

namespace Soedeum.Dotnet.Library.Data.Ranges
{
    public class RangeSet<T> : IEquatable<RangeSet<T>>
        where T : IOrderable<T>, new()
    {
        readonly Range<T>[] ranges;

        readonly int hashcode;


        protected RangeSet(params Range<T>[] ranges)
        {
            this.ranges = ranges;

            this.hashcode = HashCodes.Default.Combine(ranges);
        }

        public static readonly RangeSet<T> Empty = new RangeSet<T>(new Range<T>[0]);

        public int Count => ranges.Length;


        public bool IsEmpty => ranges.Length == 0;

        public bool IsSingle => ranges.Length == 1 && ranges[0].IsSingle;

        public bool IsRange => ranges.Length == 1 && !ranges[0].IsSingle;


        public int Find(T value) => Range<T>.Find(ranges, value);

        public bool Contains(T value) => Range<T>.Contains(ranges, value);


        public bool Equals(RangeSet<T> other)
        {
            if (this.ranges == other.ranges)
                return true;

            if (this.hashcode == other.hashcode
                && this.Count == other.Count)
            {
                for (int i = 0; i < ranges.Length; i++)
                {
                    if (this.ranges[i] != other.ranges[i])
                        return false;
                }
            }

            return true;
        }

        public override bool Equals(object other) => (other is RangeSet<T>) ? Equals(other) : false;

        public static bool operator ==(RangeSet<T> left, RangeSet<T> right) => left.Equals(right);

        public static bool operator !=(RangeSet<T> left, RangeSet<T> right) => !left.Equals(right);


        public override int GetHashCode() => hashcode;




        public string ToString(Func<T, string> toString)
        {
            bool initialized = false;

            StringBuilder builder = new StringBuilder();

            foreach (var range in ranges)
            {
                if (initialized)
                    builder.Append(" | ");

                builder.Append(range.ToString(toString));

                initialized = true;
            }

            return builder.ToString();
        }

        public override string ToString()
        {
            bool initialized = false;

            StringBuilder builder = new StringBuilder();

            foreach (var range in ranges)
            {
                if (initialized)
                    builder.Append(" | ");

                builder.Append(range.ToString());

                initialized = true;
            }

            return builder.ToString();
        }


        #region IEnumerable

        public Range<T>[] ToRangeArray() => ranges.Clone() as Range<T>[];

        public T[] ToItemArray()
        {
            var items = new List<T>();


            foreach (var range in ranges)
            {
                var current = range.Low;
                var high = range.High;

                while (current.CompareTo(high) <= 0)
                {
                    items.Add(current);

                    if (current.Equals(current.MaxValue))
                        break;
                    else
                        current = current.Next;
                }
            }

            return items.ToArray();
        }

        public IEnumerator<Range<T>> GetRangeEnumerator()
        {

            foreach (var range in ranges)
                yield return range;
        }

        public IEnumerator<T> GetItemEnumerator()
        {
            var items = new List<T>();


            foreach (var range in ranges)
            {
                var current = range.Low;
                var high = range.High;

                while (current.CompareTo(high) <= 0)
                {
                    yield return current;

                    if (current.Equals(current.MaxValue))
                        break;
                    else
                        current = current.Next;
                }
            }
        }

        public IEnumerable<T> Items => new EnumeratorGenerator<T>(GetItemEnumerator);

        public IEnumerable<Range<T>> Ranges => new EnumeratorGenerator<Range<T>>(GetRangeEnumerator);

        #endregion


        #region Constructors

        // From Value
        public static RangeSet<T> Value(T value) => new RangeSet<T>(new Range<T>(value));


        // From Range
        public static RangeSet<T> Range(Range<T> range) => new RangeSet<T>(range);

        public static RangeSet<T> Range(T a, T b) => new RangeSet<T>(new Range<T>(a, b));



        // From List
        public static RangeSet<T> List(params T[] items) => FromList(items);

        public static RangeSet<T> List(IEnumerable<T> items) => FromList(items);

        private static RangeSet<T> FromList(IEnumerable<T> codepoints)
        {
            var ranges = Range<T>.FromUnorderedList(codepoints);

            return new RangeSet<T>(ranges);
        }


        // From Union
        public static RangeSet<T> Union(params RangeSet<T>[] sets) => ReduceRanges(sets);

        public static RangeSet<T> Union(IEnumerable<RangeSet<T>> sets) => ReduceRanges(sets);

        public static RangeSet<T> Union(params Range<T>[] ranges) => ReduceRanges(ranges);

        public static RangeSet<T> Union(IEnumerable<Range<T>> ranges) => ReduceRanges(ranges);

        public static RangeSet<T> operator +(RangeSet<T> left, RangeSet<T> right) => Union(left, right);

        private static RangeSet<T> ReduceRanges(IEnumerable<RangeSet<T>> sets)
        {
            SortedSet<Range<T>> list = new SortedSet<Range<T>>();

            foreach (var set in sets)
                list.UnionWith(set.ranges);

            var reduced = Range<T>.NormalizeOrdered(list);

            return new RangeSet<T>(reduced);
        }

        private static RangeSet<T> ReduceRanges(IEnumerable<Range<T>> ranges)
        {
            var reduced = Range<T>.NormalizeUnordered(ranges);

            return new RangeSet<T>(reduced);
        }


        // From Complement
        public static RangeSet<T> Complement(RangeSet<T> set) => FromComplement(set);

        public static RangeSet<T> operator ~(RangeSet<T> set) => FromComplement(set);

        private static RangeSet<T> FromComplement(RangeSet<T> set)
        {
            var ranges = Range<T>.NormalizedComplement(set.ranges);

            return new RangeSet<T>(ranges);
        }


        // Subtraction    
        public static RangeSet<T> Remove(RangeSet<T> source, RangeSet<T> remove) => new RangeSet<T>(Range<T>.NormalizedRemove(source.ranges, remove.ranges));

        public static RangeSet<T> operator -(RangeSet<T> left, RangeSet<T> right) => Remove(left, right);

        #endregion
    }
}