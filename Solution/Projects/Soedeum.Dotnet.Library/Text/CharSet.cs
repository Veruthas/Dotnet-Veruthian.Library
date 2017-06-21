using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Collections.Enumerators;
using Soedeum.Dotnet.Library.Utility;

namespace Soedeum.Dotnet.Library.Text
{
    public class CharSet : IEquatable<CharSet>, IEnumerable<char>
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

        #region IEnumerable

        public IEnumerator<char> GetEnumerator() => GetCharEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerator<CharRange> GetRangeEnumerator()
        {

            foreach (var range in ranges)
                yield return range;
        }

        private IEnumerator<char> GetCharEnumerator()
        {
            foreach (var range in ranges)
            {
                int low = range.Low;
                int high = range.High;

                for (int i = low; i <= high; i++)
                    yield return (char)i;
            }
        }

        public IEnumerable<char> Chars => new EnumeratorGenerator<char>(GetCharEnumerator);

        public IEnumerable<CharRange> Ranges => new EnumeratorGenerator<CharRange>(GetRangeEnumerator);


        #endregion


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

        public static implicit operator CharSet(string chars) => FromList(chars);

        public static implicit operator CharSet(char[] chars) => FromList(chars);

        public static CharSet FromList(IEnumerable<char> chars)
        {
            var ranges = CharRange.FromList(chars);

            return new CharSet(ranges);
        }


        // From Union
        public static CharSet Union(params CharSet[] sets) => ReduceRanges(sets);

        public static CharSet Union(IEnumerable<CharSet> sets) => ReduceRanges(sets);

        public static CharSet Union(params CharRange[] ranges) => ReduceRanges(ranges);

        public static CharSet Union(IEnumerable<CharRange> ranges) => ReduceRanges(ranges);

        public static CharSet operator +(CharSet left, CharSet right) => Union(left, right);

        private static CharSet ReduceRanges(IEnumerable<CharSet> sets)
        {
            SortedSet<CharRange> list = new SortedSet<CharRange>();

            foreach (var set in sets)
                list.UnionWith(set.ranges);

            var reduced = CharRange.ReduceOrdered(list);

            return new CharSet(reduced);
        }

        private static CharSet ReduceRanges(IEnumerable<CharRange> ranges)
        {
            var reduced = CharRange.Reduce(ranges);

            return new CharSet(reduced);
        }

        // From Complement
        public static CharSet Complement(CharSet set) => FromComplement(set);

        public static CharSet operator ~(CharSet set) => FromComplement(set);

        private static CharSet FromComplement(CharSet set)
        {
            var ranges = CharRange.ComplementOrderedSet(set.ranges);

            return new CharSet(ranges);
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