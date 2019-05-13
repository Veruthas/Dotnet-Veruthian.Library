using System;
using System.Collections.Generic;
using Veruthian.Library.Numeric;
using Veruthian.Library.Numeric.Binary;
using Veruthian.Library.Processing;

namespace Veruthian.Library.Text.Encodings
{
    public static class Utf32
    {
        public const uint SupplementaryPlanePrefix = 0x10000;

        public const uint MinCodePoint = 0x0;

        public const uint MaxCodePoint = 0x10FFFF;


        public const uint NonCharacterStart = 0xFDD0;

        public const uint NonCharacterEnd = 0xFDEF;


        public static bool IsValid(uint value) => !IsInvalid(value);

        public static bool IsInvalid(uint value)
        {
            return ((value & 0xFFFE) == 0xFFFE)
                    || (Utf16.IsSurrogate(value))
                    || (value >= NonCharacterStart && value <= NonCharacterEnd);
        }

        public static bool IsInRange(uint value) => (value <= MaxCodePoint);

        public static bool IsOutOfRange(uint value) => (value > MaxCodePoint);


        public static bool IsInRange(int value) => (value >= 0) && (value <= MaxCodePoint);

        public static bool IsOutOfRange(int value) => (value < 0) || (value > MaxCodePoint);


        public static void VerifyIsValid(uint value)
        {
            if (IsValid(value))
                throw InvalidCodePoint(value);
        }

        public static void VerifyInRange(uint value)
        {
            if (IsOutOfRange(value))
                throw CodePointOutOfRange((int)value);
        }

        public static void VerifyInRange(int value)
        {
            if (IsOutOfRange(value))
                throw CodePointOutOfRange(value);
        }
       

        public static class Chars
        {
            public const uint Null = (uint)'\0';

            public const uint Tab = (uint)'\t';


            public const uint NewLine = (uint)'\n';

            public const uint LineFeed = (uint)'\n';

            public const uint Lf = (uint)'\n';


            public const uint CarriageReturn = (uint)'\r';

            public const uint Cr = (uint)'\r';
        }


        #region Errors

        private static EncodingException CodePointOutOfRange(int value)
        {
            return new EncodingException(CodePointOutOfRangeMessage(value));
        }

        public static string CodePointOutOfRangeMessage(int value)
        {
            return string.Format("Codepoint cannot be greater than U+10FFFF, was U+{0:X4}", value);
        }


        private static EncodingException InvalidCodePoint(uint value)
        {
            return new EncodingException(InvalidCodePointMessage(value));
        }

        public static string InvalidCodePointMessage(uint value)
        {
            return string.Format("Invalid codepoint, was U+{0:X4}", value);
        }

        #endregion

        public struct Encoder : ITransformer<uint, BitTwiddler>
        {
            bool reverse;

            public Encoder(ByteOrder endianness) => reverse = (endianness == ByteOrder.BigEndian);


            public BitTwiddler Process(uint value) => Encode(value, reverse);

            private static BitTwiddler Encode(uint value, bool reverse)
            {
                var result = BitTwiddler.FromInt((uint)value);

                if (reverse)
                    result = result.ReverseBytes();

                return result;
            }

            public static BitTwiddler Encode(uint value, ByteOrder endianness = ByteOrder.LittleEndian)
            {
                return Encode(value, endianness == ByteOrder.BigEndian);
            }
        }

        public struct Decoder : ITransformer<BitTwiddler, uint>
        {
            bool reverse;

            public Decoder(ByteOrder endianness) => reverse = (endianness == ByteOrder.BigEndian);

            public uint Process(BitTwiddler value) => Decode(value, reverse);

            private static uint Decode(BitTwiddler value, bool reverse)
            {
                if (reverse)
                    value = value.ReverseBytesInInts();

                var result = value.GetInt();

                Utf32.VerifyInRange(result);

                return result;
            }


            public static uint Decode(BitTwiddler value, ByteOrder endianness = ByteOrder.LittleEndian)
            {
                return Decode(value, endianness == ByteOrder.BigEndian);
            }
        }

        public struct ByteDecoder : ITransformer<byte, uint?>
        {
            bool isLittleEndian;

            uint state;

            int bytesRemaining;


            public ByteDecoder(ByteOrder endianness) : this()
            {
                this.isLittleEndian = endianness == ByteOrder.LittleEndian;
            }


            public uint? Process(byte value)
            {
                if (bytesRemaining == 0)
                    bytesRemaining = 3;
                else
                    bytesRemaining--;

                AddByte(value);

                if (bytesRemaining == 0)
                {
                    var result = state;

                    state = 0;

                    return result;
                }
                else
                {
                    return null;
                }
            }

            private void AddByte(uint value)
            {
                if (isLittleEndian)
                    value <<= (8 * (3 - bytesRemaining));
                else
                    state <<= 8;


                state |= value;
            }
        }
    }
}