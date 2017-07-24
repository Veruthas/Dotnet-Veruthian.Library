using System;
using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Data.Enumeration;

namespace Soedeum.Dotnet.Library.Data.Ranges
{
    public sealed class RangeSet<T> : RangeSet<T, RangeSet<T>>
        where T : IOrderable<T>, new()
    {

        public RangeSet<T> Remove(RangeSet<T> set) => Remove(this, set);

        public RangeSet<T> Complement() => Complement();


        public static bool operator ==(RangeSet<T> left, RangeSet<T> right) => left.Equals(right);

        public static bool operator !=(RangeSet<T> left, RangeSet<T> right) => !left.Equals(right);

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object other) => base.Equals(other);


        public static RangeSet<T> operator +(RangeSet<T> left, RangeSet<T> right) => Union(left, right);

        public static RangeSet<T> operator -(RangeSet<T> set, RangeSet<T> remove) => Remove(set, remove);


        public static RangeSet<T> operator ~(RangeSet<T> set) => Complement(set);
    }


    public abstract class RangeSet<T, TSet> : IEquatable<TSet>
        where T : IOrderable<T>, new()
        where TSet : RangeSet<T, TSet>, new()
    {
        static readonly Range<T>[] defaultRanges;

        static readonly int defaultHashcode;


        static RangeSet()
        {
            defaultRanges = new Range<T>[0];

            defaultHashcode = HashCodes.Default.Combine(defaultRanges);
        }

        Range<T>[] ranges;

        int hashcode;


        protected virtual void SetRanges(Range<T>[] ranges, bool clone = false)
        {
            this.ranges = clone ? (Range<T>[])ranges.Clone() : ranges;

            this.hashcode = HashCodes.Default.Combine(ranges);
        }

        public static readonly TSet Empty = new TSet();

        public int Count => ranges == null ? 0 : ranges.Length;

        private Range<T>[] RangeArray => ranges ?? defaultRanges;


        public bool IsEmpty => Count == 0;

        public bool IsSingle => Count == 1 && ranges[0].IsSingle;

        public bool IsRange => Count == 1 && !ranges[0].IsSingle;


        public int Find(T value) => Range<T>.Find(RangeArray, value);

        public bool Contains(T value) => Range<T>.Contains(RangeArray, value);


        public bool Equals(TSet other)
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

        public override bool Equals(object other) => (other is TSet) ? Equals(other) : false;


        public override int GetHashCode() => ranges == null ? defaultHashcode : hashcode;


        public string ToString(Func<T, string> toString)
        {
            bool initialized = false;

            StringBuilder builder = new StringBuilder();

            foreach (var range in RangeArray)
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

            foreach (var range in RangeArray)
            {
                if (initialized)
                    builder.Append(" | ");

                builder.Append(range.ToString());

                initialized = true;
            }

            return builder.ToString();
        }



        #region IEnumerable

        public Range<T>[] ToRangeArray() => RangeArray.Clone() as Range<T>[];

        public T[] ToItemArray()
        {
            var items = new List<T>();


            foreach (var range in RangeArray)
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
            foreach (var range in RangeArray)
                yield return range;
        }

        public IEnumerator<T> GetItemEnumerator()
        {
            var items = new List<T>();

            foreach (var range in RangeArray)
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

        private static TSet Create(params Range<T>[] ranges)
        {
            var set = new TSet();

            set.SetRanges(ranges);

            return set;
        }

        // From Value
        public static TSet Value(T value) => Create(new Range<T>(value));

        // From Range
        public static TSet Range(Range<T> range) => Create(range);

        public static TSet Range(T a, T b) => Create(new Range<T>(a, b));



        // From List
        public static TSet List(params T[] items) => FromList(items);

        public static TSet List(IEnumerable<T> items) => FromList(items);

        private static TSet FromList(IEnumerable<T> codepoints)
        {
            var ranges = Range<T>.FromUnorderedList(codepoints);

            return Create(ranges);
        }


        // From Union
        public static TSet Union(params TSet[] sets) => ReduceRanges(sets);

        public static TSet Union(IEnumerable<TSet> sets) => ReduceRanges(sets);

        public static TSet Union(params Range<T>[] ranges) => ReduceRanges(ranges);

        public static TSet Union(IEnumerable<Range<T>> ranges) => ReduceRanges(ranges);


        private static TSet ReduceRanges(IEnumerable<TSet> sets)
        {
            SortedSet<Range<T>> list = new SortedSet<Range<T>>();

            foreach (var set in sets)
                list.UnionWith(set.RangeArray);

            var reduced = Range<T>.NormalizeOrdered(list);

            return Create(reduced);
        }

        private static TSet ReduceRanges(IEnumerable<Range<T>> ranges)
        {
            var reduced = Range<T>.NormalizeUnordered(ranges);

            return Create(reduced);
        }


        // From Complement
        public static TSet Complement(TSet set) => FromComplement(set);


        private static TSet FromComplement(TSet set)
        {
            var ranges = Range<T>.NormalizedComplement(set.RangeArray);

            return Create(ranges);
        }

        // Subtraction    
        public static TSet Remove(TSet source, TSet remove) => Create(Range<T>.NormalizedRemove(source.ranges, remove.ranges));

        #endregion
    }
}