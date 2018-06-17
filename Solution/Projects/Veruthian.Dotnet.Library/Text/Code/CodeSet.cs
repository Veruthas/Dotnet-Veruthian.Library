using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Veruthian.Dotnet.Library.Data;
using Veruthian.Dotnet.Library.Data.Collections;
using Veruthian.Dotnet.Library.Data.Ranges;

namespace Veruthian.Dotnet.Library.Text.Code
{
    public class CodeSet : RangeSet<CodePoint, CodeSet>
    {
        public CodeSet Remove(CodeSet set) => Remove(this, set);

        public CodeSet Complement() => Complement();


        public static bool operator ==(CodeSet left, CodeSet right) => left.Equals(right);

        public static bool operator !=(CodeSet left, CodeSet right) => !left.Equals(right);

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object other) => base.Equals(other);


        public static CodeSet operator +(CodeSet left, CodeSet right) => Union(left, right);

        public static CodeSet operator -(CodeSet set, CodeSet remove) => Remove(set, remove);


        public static CodeSet operator ~(CodeSet set) => Complement(set);


        public static CodeSet List(CodeString codepoints) => FromList(codepoints);

        public static CodeSet List(string codepoints) => FromList(codepoints.ToCodePoints().GetEnumerableAdapter());

        // Sets            
        public static readonly CodeSet Complete = Range(CodePoint.MinValue, CodePoint.MaxValue);

        public static readonly CodeSet Null = Value('\0');

        public static readonly CodeSet NewLine = List("\n\r");

        public static readonly CodeSet SpaceOrTab = List(" \t");

        public static readonly CodeSet Whitespace = SpaceOrTab + NewLine;

        public static readonly CodeSet Lower = Range('a', 'z');

        public static readonly CodeSet Upper = Range('A', 'Z');

        public static readonly CodeSet Letter = Lower + Upper;

        public static readonly CodeSet LetterOrUnderscore = Letter + Value('_');

        public static readonly CodeSet Digit = Range('0', '9');

        public static readonly CodeSet LetterOrDigit = Letter + Digit;

        public static readonly CodeSet LetterOrDigitOrUnderscore = LetterOrDigit + Value('_');

        public static readonly CodeSet HexDigit = Union(Digit, Range('A', 'F'), Range('a', 'f'));

        public static readonly CodeSet Symbol = List("!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~");

        public static readonly CodeSet IdentifierFirst = LetterOrUnderscore;

        public static readonly CodeSet IdentifierFollow = LetterOrDigitOrUnderscore;
    }
}