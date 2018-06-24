using System;
using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Ranges
{
    public struct Range<T> : IEquatable<Range<T>>, IComparable<Range<T>>, IComparable<T>, IEnumerable<T>
        where T : ISequential<T>, new()
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
            
            // (A, C) <=> (B, C)
            if (comparison < 0)
                return -1;
            // (B, C) <=> (A, C)
            else if (comparison > 0)
                return 1;
            // (A, C) <=> (A, D)
            else
                return this.high.CompareTo(other.high);
        }

        public int CompareTo(T other)
        {
            if (low.Precedes(other))
                return -1;
            else if (high.Follows(other))
                return 1;
            else
                return 0;
        }

        public bool Contains(T value) => CompareTo(value) == 0;


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

                if (current.Equals(current.MaxValue))
                    break;
                else
                    current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        #region Range Operations

        private static T Min(T a, T b) => a.Precedes(b) ? a : b;

        private static T Max(T a, T b) => a.Follows(b) ? a : b;


        // Merge
        public static bool MergeUnordered(Range<T> a, Range<T> b, out Range<T> union)
        {
            return (a <= b)
                    ? MergeOrdered(a, b, out union)
                    : MergeOrdered(b, a, out union);
        }

        public static bool MergeOrdered(Range<T> a, Range<T> b, out Range<T> union)
        {
            // Assumes a <= b
            //  1) Disojoint (a.h + 1 < b.l)     => (a.l to a.h) + (b.l to b.h)
            //  2) Union     (a.h + 1 >= b.l)    => (a.l to b.h)

            if (a.High.Next.Precedes(b.Low))
            {
                union = default(Range<T>);

                return false;
            }
            else
            {
                var newHigh = a.high.Follows(b.high) ? a.high : b.high;

                union = new Range<T>(a.Low, newHigh);

                return true;
            }
        }

        // Intersect        
        public static bool IntersectUnordered(Range<T> a, Range<T> b, out Range<T> intersection)
        {
            return (a <= b)
                    ? IntersectOrdered(a, b, out intersection)
                    : IntersectOrdered(b, a, out intersection);
        }

        public static bool IntersectOrdered(Range<T> a, Range<T> b, out Range<T> intersection)
        {
            // 2 Possible results:
            //  1) Disojoint    (a.h < b.l)                    => (a.l to a.h) + (b.l to b.h)
            //  2) Intersection (a.l <= b.l) && (a.h <= b.h)   => (b.l to MIN(a.h, b.h))

            if (a.High.Precedes(b.Low))
            {
                intersection = default(Range<T>);

                return false;
            }
            else
            {
                T ab_high = Min(a.high, b.high);

                intersection = new Range<T>(b.low, ab_high);

                return true;
            }
        }

        // Split
        public enum SplitResultRange
        {
            Neither,
            A,
            B,
            AB
        }

        public struct SplitResult
        {
            public SplitResultRange Type { get; }

            public bool IsNeither => Type == SplitResultRange.Neither;

            public bool IsA => Type == SplitResultRange.A;

            public bool IsB => Type == SplitResultRange.B;

            public bool IsAB => Type == SplitResultRange.AB;


            public Range<T> Range { get; }

            public SplitResult(T low, T high, SplitResultRange type)
            {
                this.Range = new Range<T>(low, high);
                this.Type = type;
            }

            public SplitResult(Range<T> range, SplitResultRange type)
            {
                this.Range = range;
                this.Type = type;
            }

            public static readonly SplitResult Neither = new SplitResult();
        }

        public static bool SplitUnordered(Range<T> a, Range<T> b, out SplitResult before, out SplitResult intersection, out SplitResult after)
        {
            return (a <= b)
                    ? SplitOrdered(a, b, SplitResultRange.A, SplitResultRange.B, out before, out intersection, out after)
                    : SplitOrdered(b, a, SplitResultRange.B, SplitResultRange.A, out before, out intersection, out after);
        }

        public static bool SplitOrdered(Range<T> a, Range<T> b, out SplitResult before, out SplitResult intersection, out SplitResult after)
        {
            return SplitOrdered(a, b, SplitResultRange.A, SplitResultRange.B, out before, out intersection, out after);
        }

        private static bool SplitOrdered(Range<T> lower, Range<T> higher,
             SplitResultRange lowerRangeName, SplitResultRange higherRangeName,
             out SplitResult before, out SplitResult intersection, out SplitResult after)
        {
            // 7 Possible results
            //   1) Disjoint             (a.h < b.l)   => (a.l to a.h) + (b.l to b.h)                            -- A_X_B
            //
            //   (a.l < b.l)
            //    2) Overlap             (a.h < b.h)   => (a.l to b.l - 1) + (b.l to a.l) + (a.l + 1 to b.h)     -- A_AB_B
            //    3) SuperSet            (a.h > b.h)   => (a.l to b.l - 1) + (b.l to b.h) + (b.h + 1 to a.h)     -- A_AB_A
            //    4) SuperSetToEnd       (a.h = b.h)   => (a.l to b.l - 1) + (b.l to b.h)                        -- A_AB_X           
            //
            //   (a.l = b.l)
            //    5) SubSetFromStart     (a.h < b.h)   => (a.l to a.h)     + (a.h + 1 to b.h)                    -- X_AB_B
            //    6) SuperSetFromStart   (a.h > b.h)   => (a.l to b.h)     + (b.h + 1 to a.h)                    -- X_AB_A
            //    7) Union               (a.h = b.h)   => (a.l to b.h)                                           -- X_AB_X

            // [1] (a.h < b.l) | Disjoint => A_B
            if (lower.high.Precedes(higher.low))
            {
                before = new SplitResult(lower, lowerRangeName);

                intersection = SplitResult.Neither;

                after = new SplitResult(lower, higherRangeName);

                return false;
            }
            else
            {
                // BEFORE -> [A|X] => when a.l != b.l, (a.l to b.l - 1)
                if (lower.low.Equals(higher.low))
                    before = SplitResult.Neither;
                else
                    before = new SplitResult(lower.low, higher.low.Previous, lowerRangeName);

                // INTERSECTION -> AB => (b.l to MIN(a.h, b.h))     
                T ab_high = Min(lower.high, higher.high);

                intersection = new SplitResult(higher.low, ab_high, SplitResultRange.AB);

                // AFTER -> [A|B|X] => when a.h != b.h, (ab.h + 1 to MAX(a.h, b.h))                
                if (lower.high.Follows(higher.high))
                    after = new SplitResult(ab_high.Next, lower.high, lowerRangeName);
                else if (higher.high.Follows(lower.high))
                    after = new SplitResult(ab_high.Next, higher.high, higherRangeName);
                else
                    after = SplitResult.Neither;

                return true;
            }
        }


        #endregion

        #region Multiple Range operations

        // Find/Contains
        public static int Find(IList<Range<T>> sortedSet, T value)
        {
            int low = 0;

            int high = sortedSet.Count - 1;


            while (low <= high)
            {
                int middle = (high + low) / 2;

                var range = sortedSet[middle];

                if (value.Precedes(range.Low))
                    high = middle - 1;
                else if (value.Follows(range.High))
                    low = middle + 1;
                else
                    return middle;
            }

            return -1;
        }

        public static bool Contains(IList<Range<T>> sortedSet, T value) => Find(sortedSet, value) != -1;


        // From List
        public static Range<T>[] FromUnorderedList(IEnumerable<T> list)
        {
            SortedSet<T> sorted = new SortedSet<T>(list);

            return FromOrderedSet(sorted);
        }

        public static Range<T>[] FromOrderedSet(IEnumerable<T> set)
        {
            // Compress ranges
            List<Range<T>> ranges = new List<Range<T>>();


            bool started = false;


            T low = main.Default;

            T high = main.Default;


            foreach (var value in set)
            {
                if (!started)
                {
                    started = true;

                    low = high = value;
                }
                else
                {
                    // Add a range if this item and previous aren't consecutive
                    if (!high.Next.Equals(value))
                    {
                        var range = new Range<T>(low, high);

                        ranges.Add(range);

                        low = value;
                    }

                    high = value;
                }
            }

            // Empty list
            if (!started)
            {
                return new Range<T>[] { };
            }
            else
            {
                // Add range for remaining item(s)
                var range = new Range<T>(low, high);

                ranges.Add(range);


                return ranges.ToArray();
            }
        }

        // Normalize
        public static Range<T>[] NormalizeUnordered(IEnumerable<Range<T>> ranges)
        {
            SortedSet<Range<T>> list = new SortedSet<Range<T>>(ranges);

            return NormalizeOrdered(list);
        }

        public static Range<T>[] NormalizeOrdered(IEnumerable<Range<T>> ranges)
        {
            List<Range<T>> list = new List<Range<T>>(ranges);

            for (int i = 0; i < list.Count - 1; i++)
            {
                var low = list[i];
                var high = list[i + 1];

                if (Range<T>.MergeOrdered(low, high, out var union))
                {
                    list[i] = union;

                    // Remove reduntant range
                    list.RemoveAt(i + 1);

                    // Try to merge new range with next range                        
                    i--;
                }
            }

            return list.ToArray();
        }

        // Complement        
        public static Range<T>[] UnnormalizedComplement(IEnumerable<Range<T>> ranges)
        {
            var orderedSet = NormalizeUnordered(ranges);

            var complement = NormalizedComplement(orderedSet);

            return complement;
        }

        public static Range<T>[] NormalizedComplement(IEnumerable<Range<T>> ranges) => NormalizedComplement(ranges, main.MinValue, main.MaxValue);

        private static Range<T>[] NormalizedComplement(IEnumerable<Range<T>> ranges, T min, T max)
        {
            // Complement just creates ranges that exclude the ranges in the set
            // ex: ('A') => (Min, 'A' - 1), ('A' + 1, Max)
            var complement = new List<Range<T>>();

            T low = min;

            foreach (var range in ranges)
            {
                if (!range.Low.Equals(min))
                {
                    var newRange = new Range<T>(low.Next, range.Low.Previous);

                    complement.Add(newRange);
                }

                low = range.High;
            }

            if (!low.Equals(max))
            {
                var newLow = low.Equals(min) ? low : low.Next;

                var newRange = new Range<T>(newLow, max);

                complement.Add(newRange);
            }


            return complement.ToArray();
        }

        // Remove
        public static Range<T>[] UnnormalizedRemove(Range<T>[] ranges, params Range<T>[] removing)
        {
            ranges = NormalizeUnordered(ranges);

            removing = NormalizeUnordered(removing);

            return NormalizedRemove(ranges, removing);
        }

        public static Range<T>[] NormalizedRemove(IEnumerable<Range<T>> ranges, IEnumerable<Range<T>> removing)
        {
            // For each range in source, remove range in value;                        
            var result = new List<Range<T>>(ranges);

            foreach (var remove in removing)
                NormalizedRemove(result, remove);

            return result.ToArray();
        }

        private static void NormalizedRemove(List<Range<T>> ranges, Range<T> remove)
        {
            for (int s = 0; s < ranges.Count; s++)
            {
                var sourceRange = ranges[s];

                // There is no overlap
                if (remove.low.Follows(sourceRange.High) || remove.High.Precedes(sourceRange.Low))
                    continue;
                if (SplitUnordered(sourceRange, remove, out var before, out var intersection, out var after))
                {
                    if (before.IsA)
                    {
                        ranges[s] = before.Range;

                        if (after.IsA)
                        {
                            s++;

                            ranges.Insert(s, after.Range);
                        }
                    }
                    else if (after.IsA)
                    {
                        ranges[s] = after.Range;
                    }
                    else
                    {
                        ranges.RemoveAt(s);
                    }
                }
            }
        }

        #endregion
    }
}