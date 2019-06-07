using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Veruthian.Library.Collections;
using Veruthian.Library.Numeric;
using Veruthian.Library.Text.Runes.Extensions;

namespace Veruthian.Library.Text.Runes
{
    public class RuneSet : RangeSet<Rune, RuneSet>
    {
        public RuneSet Remove(RuneSet set) => Remove(this, set);

        public RuneSet Complement() => Complement();


        public static bool operator ==(RuneSet left, RuneSet right) => left.Equals(right);

        public static bool operator !=(RuneSet left, RuneSet right) => !left.Equals(right);

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object other) => base.Equals(other);


        public static RuneSet operator +(RuneSet left, RuneSet right) => Union(left, right);

        public static RuneSet operator -(RuneSet set, RuneSet remove) => Remove(set, remove);


        public static RuneSet List(RuneString runes) => FromList(runes);

        public static RuneSet List(string runes) => FromList(runes.ToRunes());

        // Sets            
        public static readonly RuneSet Complete = Range(Rune.MinValue, Rune.MaxValue);

        public static readonly RuneSet Null = Value('\0');

        public static readonly RuneSet NewLine = List("\n\r");

        public static readonly RuneSet TabOrSpace = List(" \t");

        public static readonly RuneSet Whitespace = TabOrSpace + NewLine;

        public static readonly RuneSet Lower = Range('a', 'z');

        public static readonly RuneSet Upper = Range('A', 'Z');

        public static readonly RuneSet Letter = Lower + Upper;

        public static readonly RuneSet LetterOrUnderscore = Letter + Value('_');

        public static readonly RuneSet Digit = Range('0', '9');

        public static readonly RuneSet LetterOrDigit = Letter + Digit;

        public static readonly RuneSet LetterOrDigitOrUnderscore = LetterOrDigit + Value('_');

        public static readonly RuneSet HexDigit = Union(Digit, Range('A', 'F'), Range('a', 'f'));

        public static readonly RuneSet Symbol = List("!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~");

        public static readonly RuneSet IdentifierFirst = LetterOrUnderscore;

        public static readonly RuneSet IdentifierFollow = LetterOrDigitOrUnderscore;
    }
}