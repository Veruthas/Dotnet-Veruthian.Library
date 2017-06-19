using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Utility;

namespace Soedeum.Dotnet.Library.Text
{
    public class CharSet
    {
        CharRange[] ranges;

        int size;

        int hashcode;

        private CharSet(params CharRange[] ranges)
        {
            this.ranges = ranges;

            foreach (var range in ranges)
                size += range.Size;

            this.hashcode = HashCodeCombiner.Combiner.Combine(ranges);
        }


        public int Size => size;


        public bool IsComplete => size == CharRange.MaxSize;

        public bool IsEmpty => size == 0;

        public bool IsRange => ranges.Length == 1 && size > 1;

        public bool IsCharacter => size == 1;

        public bool IsList => ranges.Length == size;


        public bool Contains(char value) => CharRange.Find(ranges, value) != -1;

        public int Find(char value) => CharRange.Find(ranges, value);



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


        #region Constructors

        // From Value
        public static implicit operator CharSet(char value) => new CharSet(value);

        public static CharSet Value(char value) => new CharSet(value);


        // From Range
        public static implicit operator CharSet(CharRange range) => new CharSet(range);

        public static CharSet Range(CharRange range) => new CharSet(range);

        public static CharSet Range(char low, char high) => new CharSet(new CharRange(low, high));


        // From List
        public static CharSet List(params char[] chars) => throw new NotImplementedException();

        public static CharSet List(IEnumerable<char> chars) => throw new NotImplementedException();

        public static CharSet List(string chars) => throw new NotImplementedException();

        public static implicit operator CharSet(string chars) => List(chars);


        // From Union
        public static CharSet Union(params CharSet[] sets) => throw new NotImplementedException();

        public static CharSet Union(params CharRange[] ranges) => throw new NotImplementedException();

        public static implicit operator CharSet(CharSet[] sets) => Union(sets);

        public static implicit operator CharSet(CharRange[] ranges) => Union(ranges);

        public static CharSet operator +(CharSet left, CharSet right) => Union(left, right);


        // From Complement
        public static CharSet Complement(CharSet set) => throw new NotImplementedException();

        #endregion
    }
}