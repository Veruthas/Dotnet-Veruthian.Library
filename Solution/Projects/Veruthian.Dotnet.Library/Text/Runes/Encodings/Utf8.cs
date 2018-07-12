using System;
using Veruthian.Dotnet.Library.Numeric;

namespace Veruthian.Dotnet.Library.Text.Runes.Encodings
{
    public static class Utf8
    {
        // Leading and Trailing Code Units
        // Encoding of Utf8 CodePoints
        // 1) U+0000  to U+007F (ASCII)             -> [0] 0xxx-xxxx;
        // 2) U+0080  to U+07FF                     -> [0] 110y-yyyy; [1]: 10xx-xxxx
        // 3) U+0800  to U+D7FF, U+E000 to U+FFFF   -> [0] 1110-zzzz; [1]: 10yy-yyyy; [2]: 10xx-xxxx
        // 4) U+10000 to U+10FFFF                   -> [0] 1111-0uuu; [1]: 10zz-zzzz; [2]: 10yy-yyyy; [3]: 10xx-xxxx
        //
        // Converted
        // 1) 0000-0000 0000-0000 0xxx-xxxx
        // 2) 0000-0000 0000-yyyy yyxx-xxxx
        // 3) 0000-00zz zzzz-yyyy yyxx-xxxx
        // 4) 000u-uuzz zzzy-yyyy yyxx-xxxx
        public const byte OneUnitLeadingPrefix = 0b0000_0000;
        public const byte TwoUnitLeadingPrefix = 0b1100_0000;
        public const byte ThreeUnitLeadingPrefix = 0b1110_0000;
        public const byte FourUnitLeadingPrefix = 0b1111_0000;

        public const byte OneUnitLeadingMask = 0b0111_1111;
        public const byte TwoUnitLeadingMask = 0b0001_1111;
        public const byte ThreeUnitLeadingMask = 0b0000_1111;
        public const byte FourUnitLeadingMask = 0b0000_0111;

        public const uint OneUnitMaxCodePoint = 0x7F;
        public const uint TwoUnitMaxCodePoint = 0x7FF;
        public const uint ThreeUnitMaxCodePoint = 0xFFFF;
        public const uint FourUnitMaxCodePoint = 0x10FFFF;


        private const byte TrailingUnitPrefixMask = 0b1100_0000;
        public const byte TrailingUnitPrefix = 0b1000_0000;
        public const byte TrailingUnitMask = 0b0011_1111;

        private const int TrailingUnitSize = 6;

        private const int TrailingUnitOffset0 = TrailingUnitSize * 0;
        private const int TrailingUnitOffset1 = TrailingUnitSize * 1;
        private const int TrailingUnitOffset2 = TrailingUnitSize * 2;
        private const int TrailingUnitOffset3 = TrailingUnitSize * 3;


        static readonly int[] sequenceLengths = new int[32]{
            // 0xxxx = 1  Byte Sequence (0000-0..0111-1)
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,

            // 10xxx = Invalid Sequence (1000-0..10111)
                -1, -1, -1, -1,
                -1, -1, -1, -1,

            // 110xx = 2  Byte Sequence (1100-0..1101-1)
                1, 1, 1, 1,

            // 1110x = 3  Byte Sequence (1110-0..1110-1)
                2, 2, 

            // 11110 = 4  Byte Sequence (1111-0)
                3,

            // 11111 = Invalid Sequence (1111-1)
                -1
            };

        static readonly uint[] masks = new uint[4] { OneUnitLeadingMask,
                                                     TwoUnitLeadingMask,
                                                     ThreeUnitLeadingMask,
                                                     FourUnitLeadingMask };

        public static bool ProcessLeading(byte value, out uint result, out int bytesRemaining)
        {
            // 0xxx-xxxx, 110x-xxxx, 1110-xxxx, 1111-0xxx

            // First 5 bits tell the sequence length
            int length = sequenceLengths[(value >> 3)];

            if (length == -1)
                throw InvalidCodeUnitSequence(value);

            uint mask = masks[length];

            result = value & mask;

            bytesRemaining = length;

            return bytesRemaining == 0;
        }

        public static bool ProcessTrailing(byte value, ref uint result, ref int bytesRemaining)
        {
            // Prefix MUST be 0b10xx_xxxx
            if ((value & TrailingUnitPrefixMask) != TrailingUnitPrefix)
                throw InvalidCodeUnitSequence(value);

            uint bits = (uint)(value & TrailingUnitMask);

            // Bits cannot be 0 (Utf8 must be encoded in smallest possible form)
            // if (bits == 0)
            // {
            //     Reset();

            //     throw CodePointNotSmallestSequence(value);
            // }

            // shift old bits down
            result <<= TrailingUnitSize;

            result |= bits;

            bytesRemaining--;

            return bytesRemaining == 0;
        }

        private static Exception CodePointNotSmallestSequence(byte value)
        {
            string message = "Invalid UTF8 code unit (0:X4), CodePoint must be represented in the smallest possible byte sequence.";

            return new EncodingException(string.Format(message, value.ToString()));
        }

        private static Exception InvalidCodeUnitSequence(byte value)
        {
            string message = "Invalid UTF8 code unit sequence (0:X4), invalid prefix.";
            return new EncodingException(string.Format(message, value.ToString()));
        }



        public struct Encoder : ITransformer<uint, BitTwiddler>
        {
            public BitTwiddler Process(uint value) => Encode(value);

            // 1) U+0000  to U+007F (ASCII)             -> [0] 0xxx-xxxx;
            // 2) U+0080  to U+07FF                     -> [0] 110y-yyyy; [1]: 10xx-xxxx
            // 3) U+0800  to U+D7FF, U+E000 to U+FFFF   -> [0] 1110-zzzz; [1]: 10yy-yyyy; [2]: 10xx-xxxx
            // 4) U+10000 to U+10FFFF                   -> [0] 1111-0uuu; [1]: 10zz-zzzz; [2]: 10yy-yyyy; [3]: 10xx-xxxx
            // Converted
            // 1) 0000-0000 0000-0000 0xxx-xxxx
            // 2) 0000-0000 0000-0yyy yyxx-xxxx
            // 3) 0000-0000 zzzz-yyyy yyxx-xxxx
            // 4) 000u-uuzz zzzz-yyyy yyxx-xxxx
            public static BitTwiddler Encode(uint value)
            {
                if (value <= OneUnitMaxCodePoint)
                {
                    uint unit0 = value;

                    return BitTwiddler.FromByte((byte)unit0);
                }
                else if (value <= TwoUnitMaxCodePoint)
                {
                    uint unit0 = ((value >> TrailingUnitOffset1) & TwoUnitLeadingMask) | TwoUnitLeadingPrefix;
                    uint unit1 = ((value >> TrailingUnitOffset0) & TrailingUnitMask) | TrailingUnitPrefix;

                    return BitTwiddler.FromBytes((byte)unit0, (byte)unit1);
                }
                else if (value <= ThreeUnitMaxCodePoint)
                {
                    uint unit0 = ((value >> TrailingUnitOffset2) & ThreeUnitLeadingMask) | ThreeUnitLeadingPrefix;
                    uint unit1 = ((value >> TrailingUnitOffset1) & TrailingUnitMask) | TrailingUnitPrefix;
                    uint unit2 = ((value >> TrailingUnitOffset0) & TrailingUnitMask) | TrailingUnitPrefix;

                    return BitTwiddler.FromBytes((byte)unit0, (byte)unit1, (byte)unit2);
                }
                else if (value <= FourUnitMaxCodePoint)
                {
                    uint unit0 = ((value >> TrailingUnitOffset3) & FourUnitLeadingMask) | FourUnitLeadingPrefix;
                    uint unit1 = ((value >> TrailingUnitOffset2) & TrailingUnitMask) | TrailingUnitPrefix;
                    uint unit2 = ((value >> TrailingUnitOffset1) & TrailingUnitMask) | TrailingUnitPrefix;
                    uint unit3 = ((value >> TrailingUnitOffset0) & TrailingUnitMask) | TrailingUnitPrefix;

                    return BitTwiddler.FromBytes((byte)unit0, (byte)unit1, (byte)unit2, (byte)unit3);
                }
                else
                {
                    throw new EncodingException(Utf32.CodePointOutOfRangeMessage((int)value));
                }
            }
        }

        public struct Decoder : ITransformer<BitTwiddler, uint>
        {
            public uint Process(BitTwiddler value) => Decode(value);

            public static uint Decode(BitTwiddler value)
            {
                uint result;

                int bytesRemaining;

                int i = 0;

                if (!ProcessLeading(value.GetByte(i++), out result, out bytesRemaining))
                    while (!ProcessTrailing(value.GetByte(i++), ref result, ref bytesRemaining)) ;

                return result;
            }
        }

        public struct ByteDecoder : ITransformer<byte, uint?>
        {
            uint state;

            int bytesRemaining;


            public uint? Process(byte value)
            {
                try
                {
                    if (bytesRemaining == 0)
                        ProcessLeading(value, out state, out bytesRemaining);
                    else
                        ProcessTrailing(value, ref state, ref bytesRemaining);


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
                catch
                {
                    Reset();
                    throw;
                }
            }

            private void Reset()
            {
                state = 0;

                bytesRemaining = 0;
            }
        }
    }
}