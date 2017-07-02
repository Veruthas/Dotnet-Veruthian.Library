using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Soedeum.Dotnet.Library.Utility;

namespace Soedeum.Dotnet.Library.Text
{
    [StructLayout(LayoutKind.Explicit)]
    public struct CharRange : IEquatable<CharRange>, IComparable<CharRange>, IComparable<char>, IEnumerable<char>
    {
        [FieldOffsetAttribute(0)]
        private readonly char low;


        [FieldOffsetAttribute(2)]
        private readonly char high;


        // Total => bytes: [0-1]:low, [2-3]:high
        // Problem! this only works on little-endian machines
        // Allows for easy comparison
        [FieldOffsetAttribute(0)]
        private readonly int total;


        public CharRange(char value)
        {
            total = 0;
            low = high = value;
        }

        public CharRange(char low, char high)
        {
            total = 0;

            if (low > high)
            {
                this.low = high;

                this.high = low;
            }
            else
            {
                this.low = low;

                this.high = high;
            }
        }

        public char Low => low;

        public char High => high;


        public int Size => (High - Low) + 1;


        public bool IsComplete => this == Complete;

        public bool IsCharacter => Size == 1;




        // Range to Range Comparison
        public bool Equals(CharRange other) => this.total == other.total;

        public override bool Equals(object other) => (other is CharRange) ? Equals((CharRange)other) : false;

        public static bool operator ==(CharRange left, CharRange right) => left.Equals(right);

        public static bool operator !=(CharRange left, CharRange right) => !left.Equals(right);

        public int CompareTo(CharRange other) => this.total.CompareTo(other.total);

        public static bool operator <(CharRange left, CharRange right) => left.total < right.total;

        public static bool operator >(CharRange left, CharRange right) => left.total > right.total;

        public static bool operator <=(CharRange left, CharRange right) => left.total <= right.total;

        public static bool operator >=(CharRange left, CharRange right) => left.total >= right.total;


        // Range to Char Comparison
        public bool Contains(char value) => (value >= Low) && (value <= High);

        public int CompareTo(char other)
        {
            if (other < Low)
                return 1;
            else if (other > High)
                return -1;
            else
                return 0;
        }


        // GetHashCode
        public override int GetHashCode() => total;

        // ToString
        public override string ToString()
        {
            if (IsCharacter)
                return string.Format("'{0}'", Low.GetAsPrintable());
            else
                return string.Format("('{0}' to '{1}')", Low.GetAsPrintable(), High.GetAsPrintable());
        }

        // Enumerator
        public IEnumerator<char> GetEnumerator()
        {
            int lowest = Low;
            int highest = High;
            int current = lowest;

            while (current <= highest)
            {
                yield return (char)current;
                current++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        // Complete Range Constants
        public static readonly CharRange Complete = new CharRange(char.MinValue, char.MaxValue);

        public const int MaxSize = ((int)char.MaxValue) + 1;



        #region Multiple Ranges Operations

        // Find/Contains
        public static int Find(CharRange[] sortedSet, char value)
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

        public static bool Contains(CharRange[] sortedSet, char value) => Find(sortedSet, value) != -1;


        // Range String
        public static string ToRangeString(IEnumerable<CharRange> ranges)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var range in ranges)
            {
                builder.Append(range.Low);
                builder.Append(range.High);
            }

            return builder.ToString();
        }

        public static CharRange[] FromRangeString(string rangeString, bool verifyOrderedSet = false)
        {
            if (string.IsNullOrEmpty(rangeString))
                return new CharRange[0];

            if (rangeString.Length % 2 != 0)
                throw new FormatException("Range string must contain character pairs. A lone character was found.");

            CharRange[] ranges = new CharRange[rangeString.Length / 2];

            int maxChar = -1;

            for (int i = 0, j = 0; i < rangeString.Length; i += 2, j++)
            {
                char low = rangeString[i];
                char high = rangeString[i + 1];

                if (low <= maxChar || low > high)
                    throw new FormatException("Range string must consist of a set of sorted character pairs with no ranges in common.");

                maxChar = high;

                var range = new CharRange(low, high);

                ranges[j] = range;
            }

            return ranges;
        }

        // From List
        public static CharRange[] FromList(IEnumerable<char> chars)
        {
            SortedSet<char> sortedChars = new SortedSet<char>(chars);

            return FromSet(sortedChars);
        }

        public static CharRange[] FromSet(IEnumerable<char> chars)
        {
            // Compress ranges
            List<CharRange> ranges = new List<CharRange>();


            bool started = false;


            int low = -1;

            int high = -1;


            foreach (char value in chars)
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
                        var range = new CharRange((char)low, (char)high);

                        ranges.Add(range);

                        low = value;
                    }

                    high = value;
                }
            }

            // No chars in list
            if (low == -1)
            {
                return new CharRange[] { };
            }
            else
            {
                // Add range for remaining char(s)
                var range = new CharRange((char)low, (char)high);

                ranges.Add(range);


                return ranges.ToArray();
            }
        }

        // Reduced Union
        public static CharRange[] Reduce(IEnumerable<CharRange> ranges)
        {
            SortedSet<CharRange> list = new SortedSet<CharRange>(ranges);

            return ReduceOrdered(list);
        }

        public static CharRange[] ReduceOrdered(IEnumerable<CharRange> ranges)
        {
            List<CharRange> list = new List<CharRange>(ranges);

            for (int i = 0; i < list.Count - 1; i++)
            {
                var low = list[i];
                var high = list[i + 1];

                if (CharRange.Combine(low, high, out var union))
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
        public static CharRange[] Complement(IEnumerable<CharRange> ranges)
        {
            var orderedSet = Reduce(ranges);

            var complement = ComplementOrderedSet(orderedSet);

            return complement;
        }

        public static CharRange[] ComplementOrderedSet(IEnumerable<CharRange> ranges)
        {
            // Complement just creates ranges that exclude the ranges in the set
            // ex: ('A') => (Min, 'A' - 1), ('A' + 1, Max)

            List<CharRange> complement = new List<CharRange>();


            int low = -1;

            foreach (var range in ranges)
            {
                if (range.Low != char.MinValue)
                {
                    var newRange = new CharRange((char)(low + 1), (char)(range.Low - 1));

                    complement.Add(newRange);
                }

                low = range.High;
            }

            if (low != char.MaxValue)
            {
                var newRange = new CharRange((char)(low + 1), char.MaxValue);

                complement.Add(newRange);
            }


            return complement.ToArray();
        }


        // Combine
        public static bool Combine(CharRange a, CharRange b, out CharRange union)
        {
            return (a <= b)
                    ? CombineOrdered(a, b, out union)
                    : CombineOrdered(b, a, out union);
        }

        public static bool CombineOrdered(CharRange a, CharRange b, out CharRange union)
        {
            // 2 Possible results:
            //  1) Disojoint (a.h < b.l)                    => (a.l to a.h) + (b.l to b.h)
            //  2) Union     (a.l <= b.l) && (a.h <= b.h)   => (a.l to b.h)

            if (a.High < b.Low)
            {
                union = default(CharRange);

                return false;
            }
            else
            {
                union = new CharRange(a.Low, b.High);

                return true;
            }
        }


        // Intersect        
        public static bool Intersect(CharRange a, CharRange b, out CharRange intersection)
        {
            return (a <= b)
                    ? IntersectOrdered(a, b, out intersection)
                    : IntersectOrdered(b, a, out intersection);
        }

        public static bool IntersectOrdered(CharRange a, CharRange b, out CharRange intersection)
        {
            // 2 Possible results:
            //  1) Disojoint    (a.h < b.l)                    => (a.l to a.h) + (b.l to b.h)
            //  2) Intersection (a.l <= b.l) && (a.h <= b.h)   => (b.l to MIN(a.h, b.h))

            if (a.High < b.Low)
            {
                intersection = default(CharRange);

                return false;
            }
            else
            {
                int ab_high = Math.Min(a.high, b.high);

                intersection = new CharRange(b.low, (char)ab_high);

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

            public CharRange Range { get; }

            public SplitResult(char low, char high, SplitResultSet type)
            {
                this.Range = new CharRange(low, high);
                this.Type = type;
            }

            public SplitResult(int low, int high, SplitResultSet type)
            {
                this.Range = new CharRange((char)low, (char)high);
                this.Type = type;
            }

            public SplitResult(CharRange range, SplitResultSet type)
            {
                this.Range = range;
                this.Type = type;
            }

            public static readonly SplitResult Neither = new SplitResult();
        }

        public static bool Split(CharRange a, CharRange b, out SplitResult before, out SplitResult intersection, out SplitResult after)
        {
            return (a <= b)
                    ? SplitOrdered(a, b, out before, out intersection, out after)
                    : SplitOrdered(b, a, out before, out intersection, out after);
        }

        public static bool SplitOrdered(CharRange a, CharRange b, out SplitResult before, out SplitResult intersection, out SplitResult after)
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
            if (a.high < b.low)
            {
                before = new SplitResult(a, SplitResultSet.A);

                intersection = SplitResult.Neither;

                after = new SplitResult(a, SplitResultSet.B);

                return false;
            }
            else
            {
                // BEFORE -> [A|X] => when a.l != b.l, (a.l to b.l - 1)
                before = (a.low == b.low) ? SplitResult.Neither : new SplitResult(a.low, b.low - 1, SplitResultSet.A);

                // INTERSECTION -> AB => (b.l to MIN(a.h, b.h))     
                int ab_high = Math.Min(a.high, b.high);

                intersection = new SplitResult(b.low, ab_high, SplitResultSet.AB);

                // AFTER -> [A|B|X] => when a.h != b.h, (ab.h + 1 to MAX(a.h, b.h))                
                if (a.high > b.high)
                    after = new SplitResult(ab_high + 1, a.high, SplitResultSet.A);
                else if (b.high > a.high)
                    after = new SplitResult(ab_high + 1, b.high, SplitResultSet.B);
                else
                    after = SplitResult.Neither;

                return true;
            }
        }

        #endregion
    }
}