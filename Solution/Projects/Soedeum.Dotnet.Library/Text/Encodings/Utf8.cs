using System;
using Soedeum.Dotnet.Library.Numerics;

namespace Soedeum.Dotnet.Library.Text.Encodings
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
        public const byte OneByteSequencePrefix = 0b0000_0000;
        public const byte TwoByteSequencePrefix = 0b1100_0000;
        public const byte ThreeByteSequencePrefix = 0b1110_0000;
        public const byte FourByteSequencePrefix = 0b1111_0000;

        public const uint MaxOneByteSequence = 0x7F;
        public const uint MaxTwoByteSequence = 0x7FF;
        public const uint MaxThreeByteSequence = 0xFFFF;
        public const uint MaxFourByteSequence = 0x10FFFF;


        public const byte TrailingUnitPrefix = 0b1000_0000;

        private const int TrailingUnitOffset = 6;


        /* Header */
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

        static readonly uint[] masks = new uint[4] { 0b0111_1111, 0b0001_1111, 0b0000_1111, 0b0000_0111 };

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

        /* Trailing */
        private const byte TrailingHeaderMask = 0b1100_0000;

        private const byte TrailingMask = 0b0011_1111;

        public static bool ProcessTrailing(byte value, ref uint result, ref int bytesRemaining)
        {
            // Prefix MUST be 0b10xx_xxxx
            if ((value & TrailingHeaderMask) != TrailingUnitPrefix)
                throw InvalidCodeUnitSequence(value);

            uint bits = (uint)(value & TrailingMask);

            // Bits cannot be 0 (Utf8 must be encoded in smallest possible form)
            // if (bits == 0)
            // {
            //     Reset();

            //     throw CodePointNotSmallestSequence(value);
            // }

            // shift old bits down
            result <<= TrailingUnitOffset;

            result |= bits;

            bytesRemaining--;

            return bytesRemaining == 0;
        }

        private static Exception CodePointNotSmallestSequence(byte value)
        {
            string message = "Invalid UTF8 code unit (0:X4), CodePoint must be represented in the smallest possible byte sequence.";
            return new InvalidCodePointException(string.Format(message, value.ToString()));
        }

        private static Exception InvalidCodeUnitSequence(byte value)
        {
            string message = "Invalid UTF8 code unit sequence (0:X4), invalid prefix.";
            return new InvalidCodePointException(string.Format(message, value.ToString()));
        }


        public struct Encoder : ITransformer<uint, BitTwiddler>
        {
            public BitTwiddler Process(uint value) => Encode(value);

            // 1) U+0000  to U+007F (ASCII)             -> [0] 0xxx-xxxx;
            // 2) U+0080  to U+07FF                     -> [0] 110y-yyyy; [1]: 10xx-xxxx
            // 3) U+0800  to U+D7FF, U+E000 to U+FFFF   -> [0] 1110-zzzz; [1]: 10yy-yyyy; [2]: 10xx-xxxx
            // 4) U+10000 to U+10FFFF                   -> [0] 1111-0uuu; [1]: 10zz-zzzz; [2]: 10yy-yyyy; [3]: 10xx-xxxx
            public static BitTwiddler Encode(uint value)
            {
                if (value <= MaxOneByteSequence)
                {
                    return BitTwiddler.FromByte((byte)value);
                }
                else if (value <= MaxTwoByteSequence)
                {
                    byte byte0 = (byte)((value >> 8) | TrailingUnitPrefix);
                    byte byte1 = (byte)((value >> 8) | TrailingUnitPrefix); ;

                    return BitTwiddler.FromBytes(byte0, byte1);
                }
                else if (value < MaxThreeByteSequence)
                {

                }
                else 
                {

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