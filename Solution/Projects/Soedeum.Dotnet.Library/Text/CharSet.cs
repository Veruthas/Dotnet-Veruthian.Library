using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soedeum.Dotnet.Library.Utility;

namespace Soedeum.Dotnet.Library.Text
{
    public class CharSet : IEquatable<CharSet>
    {
        readonly CharRange[] ranges;

        readonly int size;

        readonly int hashcode;


        private CharSet(params CharRange[] ranges)
        {
            this.ranges = ranges;

            foreach (var range in ranges)
                size += range.Size;

            this.hashcode = HashCodeCombiner.Combiner.Combine(ranges);
        }


        public int Size => size;


        public bool IsEmpty => size == 0;

        public bool IsCharacter => size == 1;

        public bool IsComplete => size == CharRange.MaxSize;

        public bool IsRange => ranges.Length == 1 && size > 1;

        public bool IsList => ranges.Length == size && size > 1;


        public bool Contains(char value) => CharRange.Find(ranges, value) != -1;

        public int Find(char value) => CharRange.Find(ranges, value);


        public bool Equals(CharSet other)
        {
            if (this.ranges == other.ranges)
                return true;

            if (this.hashcode == other.hashcode
                && this.size == other.size
                && this.ranges.Length == other.ranges.Length)
            {
                for (int i = 0; i < ranges.Length; i++)
                {
                    if (this.ranges[i] != other.ranges[i])
                        return false;
                }
            }

            return true;
        }

        public override bool Equals(object other) => (other is CharSet) ? Equals(other) : false;

        public static bool operator ==(CharSet left, CharSet right) => left.Equals(right);

        public static bool operator !=(CharSet left, CharSet right) => !left.Equals(right);


        public override int GetHashCode() => hashcode;


        public CharRange[] ToArray() => ranges.Clone() as CharRange[];


        public override string ToString()
        {
            if (size == 0)
                return "<none>";

            if (size == CharRange.MaxSize)
                return "<all>";

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


        #region RangeString

        public string ToRangeString() => CharRange.ToRangeString(ranges);

        public static CharSet FromRangeString(string rangeString) => new CharSet(CharRange.FromRangeString(rangeString, true));

        #endregion


        #region Constructors

        // From Value
        public static CharSet Value(char value) => new CharSet(value);

        public static implicit operator CharSet(char value) => Value(value);


        // From Range
        public static CharSet Range(CharRange range) => new CharSet(range);

        public static CharSet Range(char low, char high) => new CharSet(new CharRange(low, high));

        public static implicit operator CharSet(CharRange range) => Range(range);


        // From List
        public static CharSet List(params char[] chars) => FromList(chars);

        public static CharSet List(IEnumerable<char> chars) => FromList(chars);

        public static CharSet List(string chars) => FromList(chars);

        public static implicit operator CharSet(string chars) => List(chars);

        public static implicit operator CharSet(char[] chars) => List(chars);

        private static CharSet FromList(IEnumerable<char> chars)
        {
            // Compress ranges
            SortedSet<char> sortedChars = new SortedSet<char>(chars);

            List<CharRange> ranges = new List<CharRange>();


            if (sortedChars.Count == 0) return Empty;

            if (sortedChars.Count == CharRange.MaxSize) return Complete;


            bool started = false;

            char low = '\0';

            char high = '\0';

            CharRange range;

            foreach (char value in sortedChars)
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
                        range = new CharRange(low, high);

                        ranges.Add(range);

                        low = value;
                    }

                    high = value;
                }
            }

            // Add range for remaining char(s)
            range = new CharRange(low, high);

            ranges.Add(range);


            return new CharSet(ranges.ToArray());
        }

        // From Union

        public static CharSet Union(params CharSet[] sets) => ReduceRanges(GetSortedRangeSet(sets));

        public static CharSet Union(IEnumerable<CharSet> sets) => ReduceRanges(GetSortedRangeSet(sets));

        public static CharSet Union(params CharRange[] ranges) => ReduceRanges(GetSortedRangeSet(ranges));

        public static CharSet Union(IEnumerable<CharRange> ranges) => ReduceRanges(GetSortedRangeSet(ranges));


        public static implicit operator CharSet(CharSet[] sets) => Union(sets);

        public static implicit operator CharSet(CharRange[] ranges) => Union(ranges);

        public static CharSet operator +(CharSet left, CharSet right) => Union(left, right);

        private static SortedSet<CharRange> GetSortedRangeSet(IEnumerable<CharSet> sets)
        {
            SortedSet<CharRange> list = new SortedSet<CharRange>();

            foreach (var set in sets)
                list.UnionWith(set.ranges);

            return list;
        }

        private static SortedSet<CharRange> GetSortedRangeSet(IEnumerable<CharRange> ranges)
        {
            SortedSet<CharRange> list = new SortedSet<CharRange>(ranges);

            return list;
        }

        private static CharSet ReduceRanges(SortedSet<CharRange> ranges)
        {
            if (ranges.Count == 0)
                return Empty;
            else if (ranges.Count == 1)
            {
                var range = ranges.ElementAt(0);

                if (range.IsComplete)
                    return Complete;
                else
                    return Range(range);
            }
            else
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

                if (list.Count == 1 && list[0].IsComplete)
                    return Complete;
                else
                    return new CharSet(list.ToArray());
            }
        }

        // From Complement
        public static CharSet Complement(CharSet set) => FromComplement(set);

        public static CharSet operator ~(CharSet set) => FromComplement(set);

        private static CharSet FromComplement(CharSet set)
        {
            // Complement just creates ranges that exclude the ranges in the set
            // ex: ('A') => (Min, 'A' - 1), ('A' + 1, Max)

            if (set.IsComplete) return Empty;

            if (set.IsEmpty) return Complete;


            List<CharRange> complement = new List<CharRange>();


            int low = -1;

            foreach (var range in set.ranges)
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


            return new CharSet(complement.ToArray());
        }

        #endregion


        // Sets
        #region Sets (only using ASCII for now)

        public static readonly CharSet Complete = CharRange.Complete;

        public static readonly CharSet Empty = new CharSet();

        public static readonly CharSet Null = '\0';

        public static readonly CharSet NewLine = List("\n\r");

        public static readonly CharSet SpaceOrTab = List(" \t");

        public static readonly CharSet Whitespace = SpaceOrTab + NewLine;

        public static readonly CharSet Lower = Range('a', 'z');

        public static readonly CharSet Upper = Range('A', 'Z');

        public static readonly CharSet Letter = Lower + Upper;

        public static readonly CharSet LetterOrUnderscore = Letter + '_';

        public static readonly CharSet Digit = Range('0', '9');

        public static readonly CharSet LetterOrDigit = Letter + Digit;

        public static readonly CharSet LetterOrDigitOrUnderscore = LetterOrDigit + '_';

        public static readonly CharSet HexDigit = Union(Digit, Range('A', 'F'), Range('a', 'f'));

        public static readonly CharSet Symbol = List("!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~");

        public static readonly CharSet IdentifierFirst = LetterOrUnderscore;

        public static readonly CharSet IdentifierFollow = LetterOrDigitOrUnderscore;

        #endregion
    }
}