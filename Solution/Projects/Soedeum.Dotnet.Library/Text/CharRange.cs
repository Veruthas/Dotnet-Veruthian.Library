using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Soedeum.Dotnet.Library.Utility;

namespace Soedeum.Dotnet.Library.Text
{
    [StructLayout(LayoutKind.Explicit)]
    public struct CharRange : IEquatable<CharRange>, IComparable<CharRange>, IEnumerable<char>
    {
        [FieldOffsetAttribute(0)]
        private readonly char low;


        [FieldOffsetAttribute(2)]
        private readonly char high;


        // Total => bytes: [0-1]:low, [2-3]:high
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


        public static readonly int MaxSize = ((int)char.MaxValue) + 1;

        public bool IsComplete => this == Complete;

        public bool IsCharacter => Size == 1;


        public bool Contains(char value) => (value >= Low) && (value <= High);


        public override int GetHashCode() => total;


        public bool Equals(CharRange other) => this.total == other.total;

        public override bool Equals(object other) => (other is CharRange) ? Equals((CharRange)other) : false;

        public static bool operator ==(CharRange left, CharRange right) => left.Equals(right);

        public static bool operator !=(CharRange left, CharRange right) => !left.Equals(right);


        public int CompareTo(CharRange other) => this.total.CompareTo(other.total);


        public static bool operator <(CharRange left, CharRange right) => left.total < right.total;

        public static bool operator >(CharRange left, CharRange right) => left.total > right.total;

        public override string ToString()
        {
            if (IsCharacter)
                return string.Format("'{0}'", Low.GetAsPrintable());
            else
                return string.Format("('{0}' to '{1}')", Low.GetAsPrintable(), High.GetAsPrintable());
        }

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



        public static readonly CharRange Complete = new CharRange(char.MinValue, char.MaxValue);


        public static implicit operator CharRange(char value) => new CharRange(value);


        #region Multiple Ranges

        public static int Find(CharRange[] sortedSet, char value)
        {
            int low = 0;
            int high = sortedSet.Length - 1;

            while (low <= high)
            {
                int current = (high + low) / 2;

                var range = sortedSet[current];

                if (value < range.Low)
                    high = current - 1;
                else if (value > range.High)
                    low = current + 1;
                else
                    return current;
            }

            return -1;
        }

        public static string ToRangeString(CharRange[] ranges)
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

        #endregion

        #region Range Operation

        // Assumes a <= b
        public static bool Combine(CharRange a, CharRange b, out CharRange union)
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

        // Assumes a <= b        
        public bool Split(CharRange a, CharRange b, out SplitResult result0, out SplitResult result1, out SplitResult result2)
        {
            // 7 Possible results
            //   1) Disjoint             (a.h < b.l)   => (a.l to a.h) + (b.l to b.h)                            -- A_B
            //
            //   (a.l < b.l)
            //    2) Overlap             (a.h < b.h)   => (a.l to b.l - 1) + (b.l to a.l) + (a.l + 1 to b.h)     -- A_AB_B
            //    3) SuperSet            (a.h > b.h)   => (a.l to b.l - 1) + (b.l to b.h) + (b.h + 1 to a.h)     -- A_AB_A
            //    4) SuperSetToEnd       (a.h = b.h)   => (a.l to b.l - 1) + (b.l to b.h)                        -- A_AB            
            //
            //   (a.l = b.l)
            //    5) SubSetFromStart     (a.h < b.h)   => (a.l to a.h)     + (a.h + 1 to b.h)                    -- AB_B
            //    6) SuperSetFromStart   (a.h > b.h)   => (a.l to b.h)     + (b.h + 1 to a.h)                    -- AB_A
            //    7) Union               (a.h = b.h)   => (a.l to b.h)                                           -- AB

            // [1] (a.h < b.l) | Disjoint => A_B
            if (a.high < b.low)
            {
                result0 = result1 = result2 = SplitResult.Neither;

                return false;
            }

            // [2-4] (a.l < b.l) => A_AB_[A|B|X]
            else if (a.low < b.low)
            {
                // A => (a.l to b.l - 1)
                result0 = new SplitResult(a.low, b.low - 1, SplitResultSet.A);


                // AB => (b.l to MIN(a.h, b.h))
                int ab_high = Math.Min(a.high, b.high);

                result1 = new SplitResult(b.low, ab_high, SplitResultSet.AB);


                // [A|B|X] => when a.h != b.h, (ab.h + 1 to MAX(a.h, b.h))
                if (a.high > b.high)
                    result2 = new SplitResult(ab_high + 1, a.high, SplitResultSet.A);
                else if (b.high > a.high)
                    result2 = new SplitResult(ab_high + 1, b.high, SplitResultSet.B);
                else
                    result2 = SplitResult.Neither;
            }

            // [5-7] (a.l == b.l) => AB_[A|B|X]_X
            else
            {
                // AB => (a.l to MIN(a.h, b.h))
                int ab_high = Math.Min(a.high, b.high);

                result0 = new SplitResult(a.low, ab_high, SplitResultSet.AB);

                // [A|B|X] => when a.h != b.h, (ab.h + 1 to MAX(a.h, b.h))
                if (a.high > b.high)
                    result1 = new SplitResult(ab_high + 1, a.high, SplitResultSet.A);
                else if (b.high > a.high)
                    result1 = new SplitResult(ab_high + 1, b.high, SplitResultSet.B);
                else
                    result1 = SplitResult.Neither;

                // X
                result2 = SplitResult.Neither;
            }

            return true;
        }

        #endregion
    }
}