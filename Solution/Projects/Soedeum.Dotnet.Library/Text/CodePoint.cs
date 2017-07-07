using System;
using System.Text;
using Soedeum.Dotnet.Library.Numerics;
using Soedeum.Dotnet.Library.Text.Encodings;

namespace Soedeum.Dotnet.Library.Text
{
    public struct CodePoint : IEquatable<CodePoint>, IComparable<CodePoint>
    {
        uint value;


        private CodePoint(uint value)
        {
            Utf32.VerifyInRange(value);

            this.value = value;
        }

        private CodePoint(int value)
        {
            Utf32.VerifyInRange(value);

            this.value = (uint)value;
        }

        public bool IsValid => Utf32.IsValid(value);

        public bool IsInvalid => Utf32.IsInvalid(value);

        public void VerifyIsValid() => Utf32.VerifyIsValid(value);


        public bool IsInBmp => value < 0x10000;

        public bool IsInSmp => value >= 0x10000;


        /* Operators */
        #region Operators

        // Difference
        public static int operator -(CodePoint left, CodePoint right) => (int)left.value - (int)right.value;


        public static CodePoint operator +(CodePoint codepoint, int offset)
        {
            int value = (int)codepoint.value;

            value += offset;

            return new CodePoint(value);
        }

        public static CodePoint operator -(CodePoint codepoint, int offset)
        {
            int value = (int)codepoint.value;

            value -= offset;

            return new CodePoint(value);
        }

        public static CodePoint operator ++(CodePoint codepoint) => codepoint + 1;

        public static CodePoint operator --(CodePoint codepoint) => codepoint - 1;


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

        // For Utf8
        public static CodePoint FromUtf8(byte[] value, int index = 0) => FromUtf8(value, ref index);

        public static CodePoint FromUtf8(byte[] value, ref int index) => throw new NotImplementedException();


        // For Utf16 (U+0000 to U+D7FF, U+E000 to U+FFFF)                
        public static implicit operator CodePoint(char value) => FromUtf16(value);

        public static implicit operator CodePoint(short value) => FromUtf16(value);

        public static implicit operator CodePoint(ushort value) => FromUtf16(value);


        public static CodePoint FromUtf16(short value) => FromUtf16((ushort)value);

        public static CodePoint FromUtf16(char value) => FromUtf16((ushort)value);

        public static CodePoint FromUtf16(ushort value)
        {
            uint utf32 = value;

            return new CodePoint(utf32);
        }


        // For Utf16 Surrogate Pairs (U+10000 to U+10FFFF) 
        public static CodePoint FromUtf16(char leadingSurrogate, char trailingSurrogate) => FromUtf16((ushort)leadingSurrogate, (ushort)trailingSurrogate);

        public static CodePoint FromUtf16(short leadingSurrogate, short trailingSurrogate) => FromUtf16((ushort)leadingSurrogate, (ushort)trailingSurrogate);

        public static CodePoint FromUtf16(ushort leadingSurrogate, ushort trailingSurrogate)
        {
            uint utf32;

            utf32 = Utf16.ToUtf32(leadingSurrogate, trailingSurrogate);


            return new CodePoint(utf32);
        }

        // For Utf16
        public static CodePoint FromUtf16(short[] value, int index = 0) => FromUtf16(value, ref index);

        public static CodePoint FromUtf16(short[] value, ref int index) => throw new NotImplementedException();


        public static CodePoint FromUtf16(ushort[] value, int index = 0) => FromUtf16(value, ref index);

        public static CodePoint FromUtf16(ushort[] value, ref int index) => throw new NotImplementedException();


        public static CodePoint FromUtf16(char[] value, int index = 0) => FromUtf16(value, ref index);

        public static CodePoint FromUtf16(char[] value, ref int index) => throw new NotImplementedException();


        public static CodePoint FromUtf16(byte[] value, int index = 0, ByteOrder endianness = ByteOrder.LittleEndian) => FromUtf16(value, ref index, endianness);

        public static CodePoint FromUtf16(byte[] value, ref int index, ByteOrder endianness = ByteOrder.LittleEndian) => throw new NotImplementedException();


        // For Utf32
        public static implicit operator CodePoint(int value) => FromUtf32(value);

        public static implicit operator CodePoint(uint value) => FromUtf32(value);


        public static CodePoint FromUtf32(int value) => new CodePoint(value);

        public static CodePoint FromUtf32(uint value) => new CodePoint(value);

        public static CodePoint FromUtf32(Bits64 bits, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            var decoder = new Utf32.ByteDecoder(endianness);

            uint result = 0;

            for (int i = 0; i < 4; i++)
            {
                var value = bits.GetByte(i);

                decoder.TryProcess(value, out result);
            }

            return new CodePoint(result);
        }

        public static CodePoint FromUtf32(byte[] value, int index = 0, ByteOrder endianness = ByteOrder.LittleEndian) => FromUtf32(value, ref index, endianness);

        public static CodePoint FromUtf32(byte[] value, ref int index, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            if (index + 4 >= value.Length)
                throw new ArgumentOutOfRangeException("index", "Need 4 bytes to process to Utf32");

            var decoder = new Utf32.ByteDecoder(endianness);

            decoder.TryProcess(value[index++], out var result);
            decoder.TryProcess(value[index++], out result);
            decoder.TryProcess(value[index++], out result);
            decoder.TryProcess(value[index++], out result);

            return new CodePoint(result);
        }


        // FromString
        public static implicit operator CodePoint(string value) => FromString(value);

        public static CodePoint FromString(string value, int index = 0) => FromString(value, ref index);

        public static CodePoint FromString(string value, ref int index)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (index >= value.Length)
                throw new ArgumentOutOfRangeException("index", "Index is out of range of string.");

            char c = value[index++];

            if (Utf16.IsLeadingSurrogate(c))
            {
                if (index >= value.Length)
                    throw new ArgumentOutOfRangeException("index", "Index is out of range of string.");

                char d = value[index++];

                return CodePoint.FromUtf16(c, d);
            }
            else
            {
                return CodePoint.FromUtf16(c);
            }
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


        public Bits64 ToUtf8()
        {
            throw new NotImplementedException();
        }

        public Bits64 ToUtf16(ByteOrder endianness = ByteOrder.LittleEndian)
        {
            throw new NotImplementedException();
        }

        public Bits64 ToUtf32(ByteOrder endianness = ByteOrder.LittleEndian) => Utf32.ByteEncoder.Encode(this, endianness);


        #endregion


        /* Constants */
        #region Constants

        public static readonly CodePoint Default = new CodePoint(0);

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
