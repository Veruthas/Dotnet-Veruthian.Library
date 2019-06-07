using System;
using System.Text;
using System.Globalization;
using Veruthian.Library.Numeric;
using Veruthian.Library.Numeric.Binary;
using Veruthian.Library.Text.Encodings;

namespace Veruthian.Library.Text.Runes
{
    public struct Rune : ISequential<Rune>, IBounded<Rune>
    {
        readonly uint value;


        private Rune(uint value)
        {
            Utf32.VerifyInRange(value);

            this.value = value;
        }

        private Rune(int value)
        {
            Utf32.VerifyInRange(value);

            this.value = (uint)value;
        }

        public bool IsValid => Utf32.IsValid(value);

        public bool IsInvalid => Utf32.IsInvalid(value);

        public void VerifyIsValid() => Utf32.VerifyIsValid(value);


        public bool IsInBmp => value < 0x10000;

        public bool IsInSmp => value >= 0x10000;


        #region Operators

        // TODO: Optimize and Test
        // ToUpper/Lower        
        public Rune ToLower()
        {
            if (this.IsInBmp)
            {
                char value = (char)this;

                return char.ToLower(value);
            }
            else
            {
                string value = this;

                return value.ToLower();
            }
        }

        public Rune ToLower(CultureInfo culture)
        {
            if (this.IsInBmp)
            {
                char value = (char)this;

                return char.ToLower(value, culture);
            }
            else
            {
                string value = this;

                return value.ToLower(culture);
            }
        }

        public Rune ToLowerInvariant()
        {
            if (this.IsInBmp)
            {
                char value = (char)this;

                return char.ToLowerInvariant(value);
            }
            else
            {
                string value = this;

                return value.ToLowerInvariant();
            }
        }

        public Rune ToUpper()
        {
            if (this.IsInBmp)
            {
                char value = (char)this;

                return char.ToUpper(value);
            }
            else
            {
                string value = this;

                return value.ToUpper();
            }
        }

        public Rune ToUpper(CultureInfo culture)
        {
            if (this.IsInBmp)
            {
                char value = (char)this;

                return char.ToUpper(value, culture);
            }
            else
            {
                string value = this;

                return value.ToUpper(culture);
            }
        }

        public Rune ToUpperInvariant()
        {
            if (this.IsInBmp)
            {
                char value = (char)this;

                return char.ToUpperInvariant(value);
            }
            else
            {
                string value = this;

                return value.ToUpperInvariant();
            }
        }


        // Difference
        public static int operator -(Rune left, Rune right) => (int)left.value - (int)right.value;


        public static Rune operator +(Rune rune, int offset)
        {
            int value = (int)rune.value;

            value += offset;

            return new Rune(value);
        }

        public static Rune operator -(Rune rune, int offset)
        {
            int value = (int)rune.value;

            value -= offset;

            return new Rune(value);
        }

        public static Rune operator ++(Rune rune) => rune + 1;

        public static Rune operator --(Rune rune) => rune - 1;


        // String
        public static string operator +(string left, Rune right) => left + right.ToString();

        public static string operator +(Rune left, string right) => left.ToString() + right;

        public string ToRuneFormat()
        {
            // Format (U+[Y][Y]XXXX)
            return "U+" + value.ToString("X4");
        }

        public string ToPrintableString()
        {
            string result;

            // Convert UTF32 -> UTF16
            if (value < Utf32.SupplementaryPlanePrefix)
            {
                var asChar = (char)value;

                switch (asChar)
                {
                    case '\0':
                        result = "\\0";
                        break;
                    case '\t':
                        result = "\\t";
                        break;
                    case '\n':
                        result = "\\n";
                        break;
                    case '\r':
                        result = "\\r";
                        break;
                    default:
                        if (char.IsControl(asChar))
                            result = ToRuneFormat();
                        else
                            result = asChar.ToString();
                        break;
                }
            }
            else
            {
                result = ToString();
            }

            return result;
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


        // RuneString
        public RuneString ToRuneString() => RuneString.Of(this);

        public RuneString Replicate(int count) => RuneString.Repeat(this, count);

        public static RuneString operator *(Rune value, int count) => value.Replicate(count);


        // Equality
        public override int GetHashCode() => value.GetHashCode();

        public override bool Equals(object obj) => (obj is Rune) ? Equals((Rune)obj) : false;

        public bool Equals(Rune other) => this.value == other.value;

        public static bool operator ==(Rune left, Rune right) => left.value == right.value;

        public static bool operator !=(Rune left, Rune right) => left.value != right.value;

        public bool IsIn(RuneSet set) => set.Contains(this);

        // Comparison
        public int CompareTo(Rune other) => this.value.CompareTo(other.value);

        public static bool operator <(Rune left, Rune right) => left.value < right.value;

        public static bool operator >(Rune left, Rune right) => left.value > right.value;

        public static bool operator <=(Rune left, Rune right) => left.value <= right.value;

        public static bool operator >=(Rune left, Rune right) => left.value >= right.value;

        #endregion

        #region Constructors

        // Utf8
        public static Rune FromUtf8(BitTwiddler bits) => new Rune(Utf8.Decoder.Decode(bits));

        public static Rune FromUtf8(byte[] value, int index = 0) => FromUtf8(value, ref index);

        public static Rune FromUtf8(byte[] value, ref int index)
        {
            if (index >= value.Length)
                throw new ArgumentOutOfRangeException("index", "Index out of bounds.");

            var leading = value[index++];

            if (!Utf8.ProcessLeading(leading, out var result, out var bytesRemaining))
                while (Utf8.ProcessTrailing(leading, ref result, ref bytesRemaining)) ;

            return new Rune(result);
        }


        // Utf16        
        public static implicit operator Rune(char value) => FromUtf16(value);

        public static explicit operator Rune(short value) => FromUtf16(value);

        public static explicit operator Rune(ushort value) => FromUtf16(value);


        public static Rune FromUtf16(char value) => FromUtf16((ushort)value);

        public static Rune FromUtf16(short value) => FromUtf16((ushort)value);

        public static Rune FromUtf16(ushort value)
        {
            uint utf32 = value;

            return new Rune(utf32);
        }

        public static Rune FromUtf16(char leadingSurrogate, char trailingSurrogate) => FromUtf16((ushort)leadingSurrogate, (ushort)trailingSurrogate);

        public static Rune FromUtf16(short leadingSurrogate, short trailingSurrogate) => FromUtf16((ushort)leadingSurrogate, (ushort)trailingSurrogate);

        public static Rune FromUtf16(ushort leadingSurrogate, ushort trailingSurrogate)
        {
            uint utf32;

            utf32 = Utf16.ToUtf32(leadingSurrogate, trailingSurrogate);

            return new Rune(utf32);
        }

        public static Rune FromUtf16(BitTwiddler bits, ByteOrder endianness = ByteOrder.LittleEndian) => new Rune(Utf16.Decoder.Decode(bits, endianness));


        public static Rune FromUtf16(char[] array, int index = 0) => FromUtf16(array, ref index);

        public static Rune FromUtf16(char[] array, ref int index)
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

        public static Rune FromUtf16(short[] array, int index = 0) => FromUtf16(array, ref index);

        public static Rune FromUtf16(short[] array, ref int index)
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
                return new Rune(value);
            }
        }

        public static Rune FromUtf16(ushort[] array, int index = 0) => FromUtf16(array, ref index);

        public static Rune FromUtf16(ushort[] array, ref int index)
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
                return new Rune(value);
            }
        }

        public static Rune FromUtf16(byte[] array, int index = 0, ByteOrder endianness = ByteOrder.LittleEndian) => FromUtf16(array, ref index, endianness);

        public static Rune FromUtf16(byte[] array, ref int index, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            var decoder = new Utf16.ByteDecoder(endianness);

            if (index + 2 > array.Length)
                throw new ArgumentOutOfRangeException("index", "Need at least 2 bytes to process Utf16");

            decoder.Process(array[index++]);

            var result = decoder.Process(array[index++]);

            if (!result.Complete)
            {
                if (index + 2 > array.Length)
                    throw new ArgumentOutOfRangeException("index", "Need 4 bytes to process Utf16 surrogate pair");

                decoder.Process(array[index++]);
                result = decoder.Process(array[index++]);

                if (!result.Complete)
                    throw new RuneException(Utf16.MissingTrailingSurrogateMessage());
            }

            return new Rune(result.Result);
        }


        // Utf32
        public static explicit operator Rune(int value) => FromUtf32(value);

        public static explicit operator Rune(uint value) => FromUtf32(value);

        public static explicit operator Rune(long value) => FromUtf32(value);

        public static explicit operator Rune(ulong value) => FromUtf32(value);

        public static Rune FromUtf32(int value) => new Rune(value);

        public static Rune FromUtf32(uint value) => new Rune(value);

        public static Rune FromUtf32(BitTwiddler bits, ByteOrder endianness = ByteOrder.LittleEndian) => new Rune(Utf32.Decoder.Decode(bits, endianness));

        public static Rune FromUtf32(byte[] array, int index = 0, ByteOrder endianness = ByteOrder.LittleEndian) => FromUtf32(array, ref index, endianness);

        public static Rune FromUtf32(byte[] array, ref int index, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            if (index + 4 > array.Length)
                throw new ArgumentOutOfRangeException("index", "Need 4 bytes to process Utf32");

            var decoder = new Utf32.ByteDecoder(endianness);

            decoder.Process(array[index++]);
            decoder.Process(array[index++]);
            decoder.Process(array[index++]);
            var result = decoder.Process(array[index++]);

            return new Rune(result.Result);
        }


        // FromString
        public static implicit operator Rune(string value) => FromString(value);

        public static Rune FromString(string value, int index = 0) => FromString(value, ref index);

        public static Rune FromString(string value, ref int index)
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

                return Rune.FromUtf16(c, d);
            }
            else
            {
                return Rune.FromUtf16(c);
            }
        }

        #endregion

        #region Conversion

        public static explicit operator byte(Rune value) => (byte)value.value;

        public static explicit operator sbyte(Rune value) => (sbyte)value.value;

        public static explicit operator short(Rune value) => (short)value.value;

        public static explicit operator ushort(Rune value) => (ushort)value.value;

        public static implicit operator uint(Rune value) => value.value;

        public static implicit operator int(Rune value) => (int)value.value;

        public static implicit operator ulong(Rune value) => value.value;

        public static implicit operator long(Rune value) => value.value;

        public static explicit operator char(Rune value) => (char)value.value;

        public static implicit operator string(Rune value) => value.ToString();

        public static implicit operator RuneString(Rune value) => value.ToRuneString();


        public BitTwiddler ToUtf8() => Utf8.Encoder.Encode(this);

        public BitTwiddler ToUtf16(ByteOrder endianness = ByteOrder.LittleEndian) => Utf16.Encoder.Encode(this, endianness);

        public BitTwiddler ToUtf32(ByteOrder endianness = ByteOrder.LittleEndian) => Utf32.Encoder.Encode(this, endianness);

        #endregion

        #region Constants

        public static readonly Rune Default = new Rune(0);

        public static readonly Rune MinValue = new Rune(0);

        public static readonly Rune MaxValue = new Rune(0x10FFFF);

        public static readonly Rune Replacement = new Rune(0xFFFD);

        #endregion

        #region Sequential

        Rune ISequential<Rune>.Next => new Rune(this.value + 1);

        Rune ISequential<Rune>.Previous => new Rune(this.value - 1);


        Rune IBounded<Rune>.MinValue => Rune.MinValue;

        Rune IBounded<Rune>.MaxValue => Rune.MinValue;


        bool IOrderable<Rune>.Precedes(Rune other) => this.value < other.value;

        bool IOrderable<Rune>.Follows(Rune other) => this.value > other.value;

        #endregion
    }
}