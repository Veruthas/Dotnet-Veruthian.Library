
using System;
using System.Collections;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Text
{
    public struct CodeRange : IEquatable<CodeRange>, IComparable<CodeRange>, IComparable<CodePoint>, IEnumerable<CodePoint>
    {
        private readonly CodePoint low;

        private readonly CodePoint high;

        // Total => bytes: [0-3]:low, [4-7]:high
        private readonly ulong total;


        public CodeRange(CodePoint value)
        {
            low = high = value;

            total = (((ulong)value) << 32) | ((ulong)value);
        }

        public CodeRange(CodePoint a, CodePoint b)
        {
            if (a > b)
            {
                this.low = b;

                this.high = a;

                total = (((ulong)b) << 32) | ((ulong)a);
            }
            else
            {
                this.low = a;

                this.high = b;

                total = (((ulong)a) << 32) | ((ulong)b);
            }
        }

        public CodePoint Low => low;

        public CodePoint High => high;


        public int Size => (High - Low) + 1;


        public bool IsComplete => this == Complete;

        public bool IsCharacter => Size == 1;




        // Range to Range Comparison
        public bool Equals(CodeRange other) => this.total == other.total;

        public override bool Equals(object other) => (other is CodeRange) ? Equals((CodeRange)other) : false;

        public static bool operator ==(CodeRange left, CodeRange right) => left.Equals(right);

        public static bool operator !=(CodeRange left, CodeRange right) => !left.Equals(right);

        public int CompareTo(CodeRange other) => this.total.CompareTo(other.total);


        public static bool operator <(CodeRange left, CodeRange right) => left.total < right.total;

        public static bool operator >(CodeRange left, CodeRange right) => left.total > right.total;

        public static bool operator <=(CodeRange left, CodeRange right) => left.total <= right.total;

        public static bool operator >=(CodeRange left, CodeRange right) => left.total >= right.total;


        // Range to Char Comparison
        public bool Contains(CodePoint value) => (value >= Low) && (value <= High);

        public int CompareTo(CodePoint other)
        {
            if (other < Low)
                return 1;
            else if (other > High)
                return -1;
            else
                return 0;
        }

        private string TotalHexString => Convert.ToString((long)total, 16);

        // GetHashCode
        public override int GetHashCode() => total.GetHashCode();

        // ToString
        public override string ToString()
        {
            if (IsCharacter)
                return string.Format("'{0}'", Low.ToPrintableString());
            else
                return string.Format("('{0}' to '{1}')", Low.ToPrintableString(), High.ToPrintableString());
        }

        // Enumerator
        public IEnumerator<CodePoint> GetEnumerator()
        {
            var current = low;

            while (current <= high)
            {
                yield return current;
                current++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        // Complete Range Constants
        public static readonly CodeRange Complete = new CodeRange(CodePoint.MinValue, CodePoint.MaxValue);

        public const int MaxSize = (int)Encodings.Utf32.MaxCodePoint;



        #region Multiple Ranges Operations

        // Find/Contains
        public static int Find(CodeRange[] sortedSet, CodePoint value)
        {
            int low = 0;

            int high = sortedSet.Length - 1;


            while (low <= high)
            {
                int middle = (high + low) / 2;

                var range = sortedSet[middle];

                if (value < range.Low)
                    high = middle - 1;
                else if (value > range.High)
                    low = middle + 1;
                else
                    return middle;
            }

            return -1;
        }

        public static bool Contains(CodeRange[] sortedSet, CodePoint value) => Find(sortedSet, value) != -1;


        // RangeString
        // public static CodeString ToRangeString(IEnumerable<CodeRange> ranges)
        // {
        //     List<CodePoint> builder = new List<CodePoint>();

        //     foreach (var range in ranges)
        //     {
        //         builder.Append(range.Low);
        //         builder.Append(range.High);
        //     }

        //     return builder.ToString();
        // }

        // public static CharRange[] FromRangeString(string rangeString, bool verifyOrderedSet = false)
        // {
        //     if (string.IsNullOrEmpty(rangeString))
        //         return new CharRange[0];

        //     if (rangeString.Length % 2 != 0)
        //         throw new FormatException("Range string must contain character pairs. A lone character was found.");

        //     CharRange[] ranges = new CharRange[rangeString.Length / 2];

        //     int maxChar = -1;

        //     for (int i = 0, j = 0; i < rangeString.Length; i += 2, j++)
        //     {
        //         char low = rangeString[i];
        //         char high = rangeString[i + 1];

        //         if (low <= maxChar || low > high)
        //             throw new FormatException("Range string must consist of a set of sorted character pairs with no ranges in common.");

        //         maxChar = high;

        //         var range = new CharRange(low, high);

        //         ranges[j] = range;
        //     }

        //     return ranges;
        // }

        // From List
        public static CodeRange[] FromUnorderedList(IEnumerable<CodePoint> points)
        {
            SortedSet<CodePoint> sortedChars = new SortedSet<CodePoint>(points);

            return FromOrderedSet(sortedChars);
        }

        public static CodeRange[] FromOrderedSet(IEnumerable<CodePoint> points)
        {
            // Compress ranges
            List<CodeRange> ranges = new List<CodeRange>();


            bool started = false;


            CodePoint low = default(CodePoint);

            CodePoint high = default(CodePoint);


            foreach (var value in points)
            {
                if (!started)
                {
                    started = true;

                    low = high = value;
                }
                else
                {
                    // Add a range if this char and previous aren't consecutive
                    if (high + 1 != value)
                    {
                        var range = new CodeRange(low, high);

                        ranges.Add(range);

                        low = value;
                    }

                    high = value;
                }
            }

            // No chars in list
            if (!started)
            {
                return new CodeRange[] { };
            }
            else
            {
                // Add range for remaining points(s)
                var range = new CodeRange(low, high);

                ranges.Add(range);


                return ranges.ToArray();
            }
        }

        // Normalized ranges
        public static CodeRange[] NormalizeUnordered(IEnumerable<CodeRange> ranges)
        {
            SortedSet<CodeRange> list = new SortedSet<CodeRange>(ranges);

            return NormalizeOrdered(list);
        }

        public static CodeRange[] NormalizeOrdered(IEnumerable<CodeRange> ranges)
        {
            List<CodeRange> list = new List<CodeRange>(ranges);

            for (int i = 0; i < list.Count - 1; i++)
            {
                var low = list[i];
                var high = list[i + 1];

                if (CodeRange.CombineOrdered(low, high, out var union))
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
        public static CodeRange[] UnnormalizedComplement(IEnumerable<CodeRange> ranges)
        {
            var orderedSet = NormalizeUnordered(ranges);

            var complement = NormalizedComplement(orderedSet);

            return complement;
        }

        public static CodeRange[] NormalizedComplement(IEnumerable<CodeRange> ranges) => NormalizedComplement(ranges, CodePoint.MinValue, CodePoint.MaxValue);

        private static CodeRange[] NormalizedComplement(IEnumerable<CodeRange> ranges, CodePoint min, CodePoint max)
        {
            // Complement just creates ranges that exclude the ranges in the set
            // ex: ('A') => (Min, 'A' - 1), ('A' + 1, Max)

            List<CodeRange> complement = new List<CodeRange>();


            int low = -1;

            foreach (var range in ranges)
            {
                if (range.Low != min)
                {
                    var newRange = new CodeRange((CodePoint)(low + 1), range.Low - 1);

                    complement.Add(newRange);
                }

                low = range.High;
            }

            if (low != max)
            {
                var newRange = new CodeRange((CodePoint)(low + 1), max);

                complement.Add(newRange);
            }


            return complement.ToArray();
        }

        // Remove

        public static CodeRange[] UnnormalizedRemove(CodeRange[] ranges, params CodeRange[] removing)
        {
            ranges = NormalizeUnordered(ranges);

            removing = NormalizeUnordered(removing);

            return NormalizedRemove(ranges, removing);
        }

        public static CodeRange[] NormalizedRemove(IEnumerable<CodeRange> ranges, IEnumerable<CodeRange> removing)
        {
            // For each range in source, remove range in value;                        
            var result = new List<CodeRange>(ranges);

            foreach (var remove in removing)
                NormalizedRemove(result, remove);

            return result.ToArray();
        }

        private static void NormalizedRemove(List<CodeRange> ranges, CodeRange remove)
        {
            for (int s = 0; s < ranges.Count; s++)
            {
                var sourceRange = ranges[s];

                // There is no overlap
                if (remove.Low > sourceRange.High || remove.High < sourceRange.Low)
                    continue;
                if (CodeRange.SplitUnordered(sourceRange, remove, out var before, out var intersection, out var after))
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

        // Combine
        public static bool CombineUnordered(CodeRange a, CodeRange b, out CodeRange union)
        {
            return (a <= b)
                    ? CombineOrdered(a, b, out union)
                    : CombineOrdered(b, a, out union);
        }

        public static bool CombineOrdered(CodeRange a, CodeRange b, out CodeRange union)
        {
            // Assumes a <= b
            //  1) Disojoint (a.h + 1 < b.l)     => (a.l to a.h) + (b.l to b.h)
            //  2) Union     (a.h + 1 >= b.l)    => (a.l to b.h)

            if (a.High + 1 < b.Low)
            {
                union = default(CodeRange);

                return false;
            }
            else
            {
                var newHigh = a.high > b.high ? a.high : b.high;

                union = new CodeRange(a.Low, newHigh);

                return true;
            }
        }


        // Intersect        
        public static bool IntersectUnordered(CodeRange a, CodeRange b, out CodeRange intersection)
        {
            return (a <= b)
                    ? IntersectOrdered(a, b, out intersection)
                    : IntersectOrdered(b, a, out intersection);
        }

        public static bool IntersectOrdered(CodeRange a, CodeRange b, out CodeRange intersection)
        {
            // 2 Possible results:
            //  1) Disojoint    (a.h < b.l)                    => (a.l to a.h) + (b.l to b.h)
            //  2) Intersection (a.l <= b.l) && (a.h <= b.h)   => (b.l to MIN(a.h, b.h))

            if (a.High < b.Low)
            {
                intersection = default(CodeRange);

                return false;
            }
            else
            {
                int ab_high = Math.Min(a.high, b.high);

                intersection = new CodeRange(b.low, (char)ab_high);

                return true;
            }
        }


        // Split
        public enum SplitResultSet
        {
            Neither,
            A,
            B,
            AB
        }

        public struct SplitResult
        {
            public SplitResultSet Type { get; }

            public bool IsNeither => Type == SplitResultSet.Neither;

            public bool IsA => Type == SplitResultSet.A;

            public bool IsB => Type == SplitResultSet.B;

            public bool IsAB => Type == SplitResultSet.AB;


            public CodeRange Range { get; }

            public SplitResult(CodePoint low, CodePoint high, SplitResultSet type)
            {
                this.Range = new CodeRange(low, high);
                this.Type = type;
            }

            public SplitResult(CodeRange range, SplitResultSet type)
            {
                this.Range = range;
                this.Type = type;
            }

            public static readonly SplitResult Neither = new SplitResult();
        }

        public static bool SplitUnordered(CodeRange a, CodeRange b, out SplitResult before, out SplitResult intersection, out SplitResult after)
        {
            return (a <= b)
                    ? SplitOrdered(a, b, SplitResultSet.A, SplitResultSet.B, out before, out intersection, out after)
                    : SplitOrdered(b, a, SplitResultSet.B, SplitResultSet.A, out before, out intersection, out after);
        }

        public static bool SplitOrdered(CodeRange a, CodeRange b, out SplitResult before, out SplitResult intersection, out SplitResult after)
        {
            return SplitOrdered(a, b, SplitResultSet.A, SplitResultSet.B, out before, out intersection, out after);
        }

        private static bool SplitOrdered(CodeRange lower, CodeRange higher, SplitResultSet lowerSet, SplitResultSet higherSet,
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
            if (lower.high < higher.low)
            {
                before = new SplitResult(lower, lowerSet);

                intersection = SplitResult.Neither;

                after = new SplitResult(lower, higherSet);

                return false;
            }
            else
            {
                // BEFORE -> [A|X] => when a.l != b.l, (a.l to b.l - 1)
                before = (lower.low == higher.low) ? SplitResult.Neither : new SplitResult(lower.low, higher.low - 1, lowerSet);

                // INTERSECTION -> AB => (b.l to MIN(a.h, b.h))     
                CodePoint ab_high = (CodePoint)Math.Min(lower.high, higher.high);

                intersection = new SplitResult(higher.low, ab_high, SplitResultSet.AB);

                // AFTER -> [A|B|X] => when a.h != b.h, (ab.h + 1 to MAX(a.h, b.h))                
                if (lower.high > higher.high)
                    after = new SplitResult(ab_high + 1, lower.high, lowerSet);
                else if (higher.high > lower.high)
                    after = new SplitResult(ab_high + 1, higher.high, higherSet);
                else
                    after = SplitResult.Neither;

                return true;
            }
        }

        #endregion
    }
}