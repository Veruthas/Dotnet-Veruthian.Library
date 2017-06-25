using System.Text;

namespace Soedeum.Dotnet.Library.Text
{
    public struct CodePoint
    {
        uint value;


        private CodePoint(uint value) => this.value = value;



        // Format (U+[Y][Y]XXXX)
        public string ToCodePointString() => "U+" + value.ToString("X4");

        public override string ToString()
        {
            return null;
        }

        #region Constructors


        public static implicit operator CodePoint(char value) => FromUtf16(value);

        public static CodePoint FromUtf16(ushort value) => FromUtf16(value);

        public static CodePoint FromUtf16(short value) => FromUtf16(value);

        public static CodePoint FromUtf16(char value)
        {
            if (Utf16.IsSurrogate(value))
                throw Errors.InvalidCharacter(value);

            return new CodePoint(value);
        }

        public static CodePoint FromUtf16(char lowSurrogate, char highSurrogate)
        {

            if (!Utf16.IsHighSurrogate(highSurrogate))
                throw Errors.InvalidHighSurrogate(highSurrogate);

            if (!Utf16.IsLowSurrogate(lowSurrogate))
                throw Errors.InvalidLowSurrogate(highSurrogate);


            uint value = (uint)char.ConvertToUtf32(highSurrogate, lowSurrogate);

            return new CodePoint(value);
        }

        public static CodePoint FromUtf16(ushort lowSurrogate, ushort highSurrogate) => FromUtf16(lowSurrogate,highSurrogate);

        public static CodePoint FromUtf16(short lowSurrogate, short highSurrogate) => FromUtf16(lowSurrogate, highSurrogate);


        public static CodePoint FromUtf32(uint value)
        {
            if (value > 0x10FFFF)
                throw Errors.CodePointOutOfRange(value);

            else if (Errors.IsInvalidCodePoint(value))
                throw Errors.InvalidCodePoint(value);

            return new CodePoint(value);
        }

        public static CodePoint FromUtf32(int value) => FromUtf32((uint)value);


        #endregion

        #region Constants

        public static readonly CodePoint Max = new CodePoint(0x10FFFF);


        private static class Utf16
        {
            public static readonly uint SurrogateMin = HighSurrogateMin;

            public static readonly uint SurrogateMax = LowSurrogateMax;


            public static readonly uint HighSurrogateMin = 0xD800;

            public static readonly uint HighSurrogateMax = 0xDBFF;


            public static readonly uint LowSurrogateMin = 0xDC00;

            public static readonly uint LowSurrogateMax = 0xDFFF;


            public static bool IsHighSurrogate(uint value) => (value >= HighSurrogateMin) && (value <= HighSurrogateMax);

            public static bool IsLowSurrogate(uint value) => (value >= LowSurrogateMin) && (value <= LowSurrogateMax);

            public static bool IsSurrogate(uint value) => (value >= SurrogateMin) && (value <= SurrogateMax);
        }

        #endregion

        private static class Errors
        {
            public static bool IsInvalidCodePoint(uint value)
            {
                return (value >= 0xFDD0 && value <= 0xFDEF)
                        || ((value & 0xFFFE) == value)
                        || (Utf16.IsSurrogate(value));
            }

            public static InvalidCodePointException InvalidHighSurrogate(char value)
            {
                return new InvalidCodePointException(InvalidHighSurrogateMessage(value));
            }

            public static string InvalidHighSurrogateMessage(char value)
            {
                return string.Format("Invalid high surrogate character. Must be in range (\\uD800-\\uDBFF), was \\u{0:X4}.", value);
            }


            public static InvalidCodePointException InvalidLowSurrogate(char value)
            {
                return new InvalidCodePointException(InvalidLowSurrogateMessage(value));
            }

            public static string InvalidLowSurrogateMessage(char value)
            {
                return string.Format("Invalid low surrogate character. Must be in range (\\uDC00-\\uDFFF), was \\u{0:X4}.", value);

            }

            public static InvalidCodePointException InvalidCharacter(char value)
            {
                return new InvalidCodePointException(InvalidCharacterMessage(value));
            }

            public static string InvalidCharacterMessage(char value)
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

        public class InvalidCodePointException : System.Exception
        {
            public InvalidCodePointException() { }

            public InvalidCodePointException(string message) : base(message) { }

            public InvalidCodePointException(string message, System.Exception inner) : base(message, inner) { }

        }
    }
