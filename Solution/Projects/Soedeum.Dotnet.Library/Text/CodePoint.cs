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

        // Utf8
        public static CodePoint FromUtf8(BitTwiddler bits) => Utf8.Decoder.Decode(bits);

        public static CodePoint FromUtf8(byte[] value, int index = 0) => FromUtf8(value, ref index);

        public static CodePoint FromUtf8(byte[] value, ref int index)
        {
            if (index >= value.Length)
                throw new ArgumentOutOfRangeException("index", "Index out of bounds.");

            var leading = value[index++];

            if (!Utf8.ProcessLeading(leading, out var result, out var bytesRemaining))            
                while (Utf8.ProcessTrailing(leading, ref result, ref bytesRemaining));

            return result;
        }


        // Utf16        
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


        public static CodePoint FromUtf16(BitTwiddler bits, ByteOrder endianness = ByteOrder.LittleEndian) => Utf16.Decoder.Decode(bits, endianness);


        public static CodePoint FromUtf16(char leadingSurrogate, char trailingSurrogate) => FromUtf16((ushort)leadingSurrogate, (ushort)trailingSurrogate);

        public static CodePoint FromUtf16(short leadingSurrogate, short trailingSurrogate) => FromUtf16((ushort)leadingSurrogate, (ushort)trailingSurrogate);

        public static CodePoint FromUtf16(ushort leadingSurrogate, ushort trailingSurrogate)
        {
            uint utf32;

            utf32 = Utf16.ToUtf32(leadingSurrogate, trailingSurrogate);

            return new CodePoint(utf32);
        }

        public static CodePoint FromUtf16(short[] array, int index = 0) => FromUtf16(array, ref index);

        public static CodePoint FromUtf16(short[] array, ref int index)
        {
            if (index >= array.Length)
                throw new ArgumentOutOfRangeException("index", "Index out of bounds.");

            var value = array[index++];

            if (Utf16.IsLeadingSurrogate((ushort)value))
            {
                if (index >= array.Length)
                    throw new ArgumentOutOfRangeException("index", "Need two items to process surrogate pair.");

                var trailing = array[index++];

                return FromUtf16(value, trailing);
            }
            else
            {
                return value;
            }
        }


        public static CodePoint FromUtf16(ushort[] array, int index = 0) => FromUtf16(array, ref index);

        public static CodePoint FromUtf16(ushort[] array, ref int index)
        {
            if (index >= array.Length)
                throw new ArgumentOutOfRangeException("index", "Index out of bounds.");

            var value = array[index++];

            if (Utf16.IsLeadingSurrogate(value))
            {
                if (index >= array.Length)
                    throw new ArgumentOutOfRangeException("index", "Need two items to process surrogate pair.");

                var trailing = array[index++];

                return FromUtf16(value, trailing);
            }
            else
            {
                return value;
            }
        }


        public static CodePoint FromUtf16(char[] array, int index = 0) => FromUtf16(array, ref index);

        public static CodePoint FromUtf16(char[] array, ref int index)
        {
            if (index >= array.Length)
                throw new ArgumentOutOfRangeException("index", "Index out of bounds.");

            var value = array[index++];

            if (Utf16.IsLeadingSurrogate(value))
            {
                if (index >= array.Length)
                    throw new ArgumentOutOfRangeException("index", "Need two items to process surrogate pair.");

                var trailing = array[index++];

                return FromUtf16(value, trailing);
            }
            else
            {
                return value;
            }
        }


        public static CodePoint FromUtf16(byte[] array, int index = 0, ByteOrder endianness = ByteOrder.LittleEndian) => FromUtf16(array, ref index, endianness);

        public static CodePoint FromUtf16(byte[] array, ref int index, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            var decoder = new Utf16.ByteDecoder(endianness);

            if (index + 2 > array.Length)
                throw new ArgumentOutOfRangeException("index", "Need at least 2 bytes to process Utf16");

            decoder.Process(array[index++]);
            var result = decoder.Process(array[index++]);

            if (result == null)
            {
                if (index + 2 > array.Length)
                    throw new ArgumentOutOfRangeException("index", "Need 4 bytes to process Utf16 surrogate pair");

                decoder.Process(array[index++]);
                result = decoder.Process(array[index++]);

                if (result == null)
                    throw new CodePointException(Utf16.MissingTrailingSurrogateMessage());
            }

            return result.GetValueOrDefault();
        }


        // Utf32
        public static implicit operator CodePoint(int value) => FromUtf32(value);

        public static implicit operator CodePoint(uint value) => FromUtf32(value);


        public static CodePoint FromUtf32(int value) => new CodePoint(value);

        public static CodePoint FromUtf32(uint value) => new CodePoint(value);

        public static CodePoint FromUtf32(BitTwiddler bits, ByteOrder endianness = ByteOrder.LittleEndian) => Utf32.Decoder.Decode(bits, endianness);

        public static CodePoint FromUtf32(byte[] array, int index = 0, ByteOrder endianness = ByteOrder.LittleEndian) => FromUtf32(array, ref index, endianness);

        public static CodePoint FromUtf32(byte[] array, ref int index, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            if (index + 4 > array.Length)
                throw new ArgumentOutOfRangeException("index", "Need 4 bytes to process Utf32");

            var decoder = new Utf32.ByteDecoder(endianness);

            decoder.Process(array[index++]);
            decoder.Process(array[index++]);
            decoder.Process(array[index++]);
            var result = decoder.Process(array[index++]);

            return new CodePoint(result.GetValueOrDefault());
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


        public BitTwiddler ToUtf8() => Utf8.Encoder.Encode(this);

        public BitTwiddler ToUtf16(ByteOrder endianness = ByteOrder.LittleEndian) => Utf16.Encoder.Encode(this, endianness);

        public BitTwiddler ToUtf32(ByteOrder endianness = ByteOrder.LittleEndian) => Utf32.Encoder.Encode(this, endianness);


        #endregion


        /* Constants */
        #region Constants

        public static readonly CodePoint Default = new CodePoint(0);

        public static readonly CodePoint MinValue = new CodePoint(0);

        public static readonly CodePoint MaxValue = new CodePoint(0x10FFFF);

        public static readonly CodePoint Replacement = new CodePoint(0xFFFD);

        #endregion

    }


    public class CodePointException : System.Exception
    {
        public CodePointException() { }

        public CodePointException(string message) : base(message) { }

        public CodePointException(string message, System.Exception inner) : base(message, inner) { }
    }
}
