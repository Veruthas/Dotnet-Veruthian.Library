using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Utility;

namespace Soedeum.Dotnet.Library.Text
{
    public class CodeSet : IEquatable<CodeSet>
    {
        readonly CodeRange[] ranges;

        readonly int size;

        readonly int hashcode;


        private CodeSet(params CodeRange[] ranges)
        {
            this.ranges = ranges;

            foreach (var range in ranges)
                size += range.Size;

            this.hashcode = HashCodeCreator.Combiner.Combine(ranges);
        }


        public int Size => size;


        public bool IsEmpty => size == 0;

        public bool IsCharacter => size == 1;

        public bool IsComplete => size == CodeRange.MaxSize;

        public bool IsRange => ranges.Length == 1 && size > 1;

        public bool IsList => ranges.Length == size && size > 1;


        public bool Contains(CodePoint value) => CodeRange.Find(ranges, value) != -1;

        public int Find(CodePoint value) => CodeRange.Find(ranges, value);


        public bool Equals(CodeSet other)
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

        public override bool Equals(object other) => (other is CodeSet) ? Equals(other) : false;

        public static bool operator ==(CodeSet left, CodeSet right) => left.Equals(right);

        public static bool operator !=(CodeSet left, CodeSet right) => !left.Equals(right);


        public override int GetHashCode() => hashcode;


        public CodeRange[] ToArray() => ranges.Clone() as CodeRange[];


        public override string ToString()
        {
            if (size == 0)
                return "<none>";

            if (size == CodeRange.MaxSize)
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

        private IEnumerator<CodeRange> GetRangeEnumerator()
        {

            foreach (var range in ranges)
                yield return range;
        }

        private IEnumerator<CodePoint> GetCharEnumerator()
        {
            foreach (var range in ranges)
            {
                int low = range.Low;
                int high = range.High;

                for (int i = low; i <= high; i++)
                    yield return (CodePoint)i;
            }
        }

        public IEnumerable<CodePoint> CodePoints => new EnumeratorGenerator<CodePoint>(GetCharEnumerator);

        public IEnumerable<CodeRange> Ranges => new EnumeratorGenerator<CodeRange>(GetRangeEnumerator);


        #endregion


        #region RangeString

        // public string ToRangeString() => CodeRange.ToRangeString(ranges);

        // public static CodeSet FromRangeString(string rangeString) => new CodeSet(CodeRange.FromRangeString(rangeString, true));

        #endregion


        #region Constructors

        // From Value
        public static CodeSet Value(CodePoint value) => new CodeSet(new CodeRange(value));

        public static implicit operator CodeSet(CodePoint value) => Value(value);

        public static implicit operator CodeSet(char value) => Value(value);


        // From Range
        public static CodeSet Range(CodeRange range) => new CodeSet(range);

        public static CodeSet Range(CodePoint low, CodePoint high) => new CodeSet(new CodeRange(low, high));

        public static implicit operator CodeSet(CodeRange range) => Range(range);


        // From List
        public static CodeSet List(params CodePoint[] codepoints) => FromList(codepoints);

        public static CodeSet List(IEnumerable<CodePoint> codepoints) => FromList(codepoints);

        public static CodeSet List(CodeString codepoints) => FromList(codepoints);

        public static CodeSet List(string codepoints) => FromList(codepoints.ToCodePoints());

        public static implicit operator CodeSet(string codepoints) => List(codepoints);

        public static implicit operator CodeSet(CodeString codepoints) => List(codepoints);

        public static implicit operator CodeSet(CodePoint[] codepoints) => List(codepoints);

        private static CodeSet FromList(IEnumerable<CodePoint> codepoints)
        {
            var ranges = CodeRange.FromList(codepoints);

            return new CodeSet(ranges);
        }


        // From Union
        public static CodeSet Union(params CodeSet[] sets) => ReduceRanges(sets);

        public static CodeSet Union(IEnumerable<CodeSet> sets) => ReduceRanges(sets);

        public static CodeSet Union(params CodeRange[] ranges) => ReduceRanges(ranges);

        public static CodeSet Union(IEnumerable<CodeRange> ranges) => ReduceRanges(ranges);

        public static CodeSet operator +(CodeSet left, CodeSet right) => Union(left, right);

        private static CodeSet ReduceRanges(IEnumerable<CodeSet> sets)
        {
            SortedSet<CodeRange> list = new SortedSet<CodeRange>();

            foreach (var set in sets)
                list.UnionWith(set.ranges);

            var reduced = CodeRange.ReduceOrdered(list);

            return new CodeSet(reduced);
        }

        private static CodeSet ReduceRanges(IEnumerable<CodeRange> ranges)
        {
            var reduced = CodeRange.Reduce(ranges);

            return new CodeSet(reduced);
        }

        // From Complement
        public static CodeSet Complement(CodeSet set) => FromComplement(set);

        public static CodeSet operator ~(CodeSet set) => FromComplement(set);

        private static CodeSet FromComplement(CodeSet set)
        {
            var ranges = CodeRange.ComplementOrderedSet(set.ranges);

            return new CodeSet(ranges);
        }


        // Subtraction    
        public static CodeSet Remove(CodeSet source, CodeSet removing)
        {
            // TODO: Heavily optimize

            // For each range in source, remove range in value;            
            var removingRanges = removing.ranges;

            var result = new List<CodeRange>(source.ranges);

            int min = 0;

            for (int r = 0; r < removingRanges.Length; r++)
            {
                var removeRange = removingRanges[r];

                for (int s = min; s < result.Count; s++)
                {
                    var sourceRange = result[s];

                    // There is no overlap
                    if (removeRange.Low > sourceRange.High || removeRange.High < sourceRange.Low)
                        continue;
                    if (CodeRange.Split(sourceRange, removeRange, out var before, out var intersection, out var after))
                    {
                        if (before.IsA)
                        {
                            result[s] = before.Range;

                            if (after.IsA)
                            {
                                s++;

                                result.Insert(s, after.Range);
                            }
                        }
                        else if (after.IsA)
                        {
                            result[s] = after.Range;
                        }
                        else
                        {
                            result.RemoveAt(s);
                        }
                    }
                }
            }

            return new CodeSet(result.ToArray());
        }

        public static CodeSet operator -(CodeSet left, CodeSet right) => Remove(left, right);

        #endregion


        // Sets
        #region Sets (only using ASCII for now)

        public static readonly CodeSet Complete = CodeRange.Complete;

        public static readonly CodeSet Empty = new CodeSet();

        public static readonly CodeSet Null = '\0';

        public static readonly CodeSet NewLine = List("\n\r");

        public static readonly CodeSet SpaceOrTab = List(" \t");

        public static readonly CodeSet Whitespace = SpaceOrTab + NewLine;

        public static readonly CodeSet Lower = Range('a', 'z');

        public static readonly CodeSet Upper = Range('A', 'Z');

        public static readonly CodeSet Letter = Lower + Upper;

        public static readonly CodeSet LetterOrUnderscore = Letter + '_';

        public static readonly CodeSet Digit = Range('0', '9');

        public static readonly CodeSet LetterOrDigit = Letter + Digit;

        public static readonly CodeSet LetterOrDigitOrUnderscore = LetterOrDigit + '_';

        public static readonly CodeSet HexDigit = Union(Digit, Range('A', 'F'), Range('a', 'f'));

        public static readonly CodeSet Symbol = List("!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~");

        public static readonly CodeSet IdentifierFirst = LetterOrUnderscore;

        public static readonly CodeSet IdentifierFollow = LetterOrDigitOrUnderscore;

        #endregion
    }
}