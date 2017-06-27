using System;
using System.Text;

namespace Soedeum.Dotnet.Library.Text
{
    public struct CodePoint : IEquatable<CodePoint>, IComparable<CodePoint>
    {
        uint value;


        private CodePoint(uint value) => this.value = value;


        /* Operators */
        #region Operators

        // String
        public static string operator +(string left, CodePoint right) => left + right.ToString();

        public static string operator +(CodePoint left, string right) => left.ToString() + right;

        public string ToCodePointString()
        {
            // Format (U+[Y][Y]XXXX)
            return "U+" + value.ToString("X4");
        }

        public override string ToString()
        {
            string result;

            // Convert UTF32 -> UTF16
            if (value < Utf32.SupplementaryPlanePrefix)
            {
                result = ((char)value).ToString();
            }
            else
            {
                Utf16.SplitSurrogates(value, out ushort leading, out ushort trailing);

                result = "" + (char)leading + (char)trailing;
            }

            return result;
        }



        // Equality
        public override int GetHashCode() => value.GetHashCode();

        public override bool Equals(object obj) => (obj is CodePoint) ? Equals((CodePoint)obj) : false;

        public bool Equals(CodePoint other) => this.value == other.value;

        public static bool operator ==(CodePoint left, CodePoint right) => left.value == right.value;

        public static bool operator !=(CodePoint left, CodePoint right) => left.value == right.value;


        // Comparison
        public int CompareTo(CodePoint other) => this.value.CompareTo(other.value);

        public static bool operator <(CodePoint left, CodePoint right) => left.value < right.value;

        public static bool operator >(CodePoint left, CodePoint right) => left.value > right.value;

        public static bool operator <=(CodePoint left, CodePoint right) => left.value <= right.value;

        public static bool operator >=(CodePoint left, CodePoint right) => left.value >= right.value;

        #endregion


        /* Constructors */
        #region Constructors        

        // For Utf16 (U+0000 to U+D7FF, U+E000 to U+FFFF)                
        public static implicit operator CodePoint(char value) => FromUtf16(value);

        public static implicit operator CodePoint(short value) => FromUtf16(value);

        public static implicit operator CodePoint(ushort value) => FromUtf16(value);


        public static CodePoint FromUtf16(short value) => FromUtf16((ushort)value);

        public static CodePoint FromUtf16(char value) => FromUtf16((ushort)value);

        public static CodePoint FromUtf16(ushort value)
        {
            if (Utf16.IsSurrogate(value))
                throw Errors.InvalidCharacter(value);

            return new CodePoint(value);
        }


        // For Utf16 Surrogate Pairs (U+10000 to U+10FFFF) 
        public static CodePoint FromUtf16(char leadingSurrogate, char trailingSurrogate) => FromUtf16((ushort)leadingSurrogate, (ushort)trailingSurrogate);

        public static CodePoint FromUtf16(short leadingSurrogate, short trailingSurrogate) => FromUtf16((ushort)leadingSurrogate, (ushort)trailingSurrogate);

        public static CodePoint FromUtf16(ushort leadingSurrogate, ushort trailingSurrogate)
        {
            if (!Utf16.IsLeadingSurrogate(leadingSurrogate))
                throw Errors.InvalidLeadingSurrogate(leadingSurrogate);

            if (!Utf16.IsLowSurrogate(trailingSurrogate))
                throw Errors.InvalidTrailingSurrogate(leadingSurrogate);


            uint value = Utf16.CombineSurrogates(leadingSurrogate, trailingSurrogate);

            return new CodePoint(value);
        }

        // For Utf32
        public static implicit operator CodePoint(int value) => FromUtf32(value);

        public static implicit operator CodePoint(uint value) => FromUtf32(value);


        public static CodePoint FromUtf32(int value) => FromUtf32((uint)value);

        public static CodePoint FromUtf32(uint value)
        {
            if (value > 0x10FFFF)
                throw Errors.CodePointOutOfRange(value);

            else if (Utf32.IsInvalid(value))
                throw Errors.InvalidCodePoint(value);

            return new CodePoint(value);
        }

        #endregion


        /* Conversion */
        #region Conversion

        public static explicit operator char(CodePoint value) => (char)value.value;

        public static explicit operator byte(CodePoint value) => (byte)value.value;

        public static explicit operator sbyte(CodePoint value) => (sbyte)value.value;

        public static explicit operator short(CodePoint value) => (short)value.value;

        public static explicit operator ushort(CodePoint value) => (ushort)value.value;

        public static implicit operator uint(CodePoint value) => value.value;

        public static implicit operator int(CodePoint value) => (int)value.value;

        public static implicit operator ulong(CodePoint value) => value.value;

        public static implicit operator long(CodePoint value) => value.value;

        public static explicit operator string(CodePoint value) => value.ToString();

        #endregion



        /* Constants */
        #region Constants

        public static readonly CodePoint Max = new CodePoint(0x10FFFF);

        public static readonly CodePoint Min = new CodePoint(0);

        public static readonly CodePoint Replacement = new CodePoint(0xFFFD);

        #endregion

        /* Helper Classes */
        #region Helper Classes

        private static class Errors
        {
            public static InvalidCodePointException InvalidLeadingSurrogate(ushort value)
            {
                return new InvalidCodePointException(InvalidLeadingSurrogateMessage(value));
            }

            public static string InvalidLeadingSurrogateMessage(ushort value)
            {
                return string.Format("Invalid leading surrogate character. Must be in range (\\uD800-\\uDBFF), was \\u{0:X4}.", value);
            }


            public static InvalidCodePointException InvalidTrailingSurrogate(ushort value)
            {
                return new InvalidCodePointException(InvalidTrailingSurrogateMessage(value));
            }

            public static string InvalidTrailingSurrogateMessage(ushort value)
            {
                return string.Format("Invalid trailing surrogate character. Must be in range (\\uDC00-\\uDFFF), was \\u{0:X4}.", value);

            }

            public static InvalidCodePointException InvalidCharacter(ushort value)
            {
                return new InvalidCodePointException(InvalidCharacterMessage(value));
            }

            public static string InvalidCharacterMessage(ushort value)
            {
                return string.Format("Character cannot be a surrogate (in range \\uD800-\\uDFFF), was \\u{0:X4}", value);
            }

            public static InvalidCodePointException InvalidCodePoint(uint value)
            {
                return new InvalidCodePointException(CodePointOutOfRangeMessage(value));
            }

            public static string InvalidCodePointMessage(uint value)
            {
                return string.Format("Invalid codepoint, was U+{0:X4}", value);
            }

            public static InvalidCodePointException CodePointOutOfRange(uint value)
            {
                return new InvalidCodePointException(CodePointOutOfRangeMessage(value));
            }

            public static string CodePointOutOfRangeMessage(uint value)
            {
                return string.Format("Codepoint cannot be greater than U+10FFFF, was U+{0:X4}", value);
            }
        }

        #endregion
    }


    public class InvalidCodePointException : System.Exception
    {
        public InvalidCodePointException() { }

        public InvalidCodePointException(string message) : base(message) { }

        public InvalidCodePointException(string message, System.Exception inner) : base(message, inner) { }

    }
}
