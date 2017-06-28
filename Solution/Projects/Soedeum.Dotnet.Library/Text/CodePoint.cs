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
                Utf16.FromUtf32(value, out ushort leading, out ushort trailing);

                result = "" + (char)leading + (char)trailing;
            }

            return result;
        }

        // FromString
        public static CodePoint[] FromString(string value)
        {
            if (value == null)
                return null;
            else
                return FromString(value, 0, value.Length);
        }

        public static CodePoint[] FromString(string value, int start)
        {
            if (value == null)
                return null;
            else
                return FromString(value, start, value.Length - start);
        }

        public static CodePoint[] FromString(string value, int start, int amount)
        {
            if (value == null)
                return null;

            if (start < 0 || start > value.Length)
                throw new ArgumentOutOfRangeException("start");

            if (amount < 0 || start + amount > value.Length)
                throw new ArgumentOutOfRangeException("amount");


            var codepoints = new CodePoint[amount];

            int index = 0;


            var decoder = new Utf16.CharDecoder();

            bool result = true;

            for (int i = start; i < amount; i++)
            {
                result = decoder.TryProcess(value[i], out var codepoint);

                if (result)
                    codepoints[index++] = codepoint;
            }

            if (!result)
                throw new InvalidCodePointException("Missing trailing surrogate");

            if (index < codepoints.Length)
                Array.Resize(ref codepoints, index);

            return codepoints;
        }


        // CodeString
        public static CodeString operator *(CodePoint value, int count) => new CodeString(value, count);


        // Equality
        public override int GetHashCode() => value.GetHashCode();

        public override bool Equals(object obj) => (obj is CodePoint) ? Equals((CodePoint)obj) : false;

        public bool Equals(CodePoint other) => this.value == other.value;

        public static bool operator ==(CodePoint left, CodePoint right) => left.value == right.value;

        public static bool operator !=(CodePoint left, CodePoint right) => left.value != right.value;


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
            var utf32 = Utf16.ToUtf32(value);

            return new CodePoint(value);
        }


        // For Utf16 Surrogate Pairs (U+10000 to U+10FFFF) 
        public static CodePoint FromUtf16(char leadingSurrogate, char trailingSurrogate) => FromUtf16((ushort)leadingSurrogate, (ushort)trailingSurrogate);

        public static CodePoint FromUtf16(short leadingSurrogate, short trailingSurrogate) => FromUtf16((ushort)leadingSurrogate, (ushort)trailingSurrogate);

        public static CodePoint FromUtf16(ushort leadingSurrogate, ushort trailingSurrogate)
        {
            uint value = Utf16.ToUtf32(leadingSurrogate, trailingSurrogate);

            return new CodePoint(value);
        }

        // For Utf32
        public static implicit operator CodePoint(int value) => FromUtf32(value);

        public static implicit operator CodePoint(uint value) => FromUtf32(value);


        public static CodePoint FromUtf32(int value) => FromUtf32((uint)value);

        public static CodePoint FromUtf32(uint value)
        {
            Utf32.Verify(value);

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

        public static readonly CodePoint MinValue = new CodePoint(0);

        public static readonly CodePoint MaxValue = new CodePoint(0x10FFFF);

        public static readonly CodePoint Replacement = new CodePoint(0xFFFD);

        #endregion

    }


    public class InvalidCodePointException : System.Exception
    {
        public InvalidCodePointException() { }

        public InvalidCodePointException(string message) : base(message) { }

        public InvalidCodePointException(string message, System.Exception inner) : base(message, inner) { }
    }
}
