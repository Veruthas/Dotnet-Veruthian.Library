using System;
using Veruthian.Library.Numeric;
using Veruthian.Library.Numeric.Binary;
using Veruthian.Library.Processing;

namespace Veruthian.Library.Text.Encodings
{
    public static class Utf16
    {
        public const ushort LeadingSurrogateMin = 0xD800;

        public const ushort LeadingSurrogateMax = 0xDBFF;


        public const ushort TrailingSurrogateMin = 0xDC00;

        public const ushort TrailingSurrogateMax = 0xDFFF;


        public const ushort SurrogateMin = LeadingSurrogateMin;

        public const ushort SurrogateMax = TrailingSurrogateMax;


        // 0000-0011 1111-1111
        public const ushort SurrogateMask = 0x03FF;

        public const int LeadingSurrogateOffset = 10;

        // 110110 wwwwxxxxxx    =>  [1]: 1101-10ww [0]: wwxx-xxxx
        public const ushort LeadingSurrogatePrefix = 0xD800;

        // 110111 yyyyyyyyyy    =>  [1]: 1101-11yy [0]: yyyy-yyyy
        public const ushort TrailingSurrogatePrefix = 0xDC00;


        public static bool IsLeadingSurrogate(uint value) => (value >= LeadingSurrogateMin) && (value <= LeadingSurrogateMax);

        public static bool IsTrailingSurrogate(uint value) => (value >= TrailingSurrogateMin) && (value <= TrailingSurrogateMax);

        public static bool IsSurrogate(uint value) => (value >= SurrogateMin) && (value <= SurrogateMax);


        // To Utf32
        public static uint ToUtf32(char value) => ToUtf32((ushort)value);

        public static uint ToUtf32(short value) => ToUtf32((ushort)value);

        public static uint ToUtf32(ushort value)
        {
            if (IsSurrogate(value))
                throw InvalidCharacter(value);

            return value;
        }

        public static uint ToUtf32(char leadingSurrogate, char trailingSurrogate) => ToUtf32((ushort)leadingSurrogate, (ushort)trailingSurrogate);

        public static uint ToUtf32(short leadingSurrogate, short trailingSurrogate) => ToUtf32((ushort)leadingSurrogate, (ushort)trailingSurrogate);

        public static uint ToUtf32(ushort leadingSurrogate, ushort trailingSurrogate)
        {
            if (!Utf16.IsLeadingSurrogate(leadingSurrogate))
                throw InvalidLeadingSurrogate(leadingSurrogate);

            if (!Utf16.IsTrailingSurrogate(trailingSurrogate))
                throw InvalidTrailingSurrogate(trailingSurrogate);

            return CombineSurrogates(leadingSurrogate, trailingSurrogate);
        }

        public static uint CombineSurrogates(ushort leadingSurrogate, ushort trailingSurrogate)
        {
            // Remove high surrogate prefix, and shift left
            uint high = ((uint)leadingSurrogate & SurrogateMask) << LeadingSurrogateOffset;

            // Remove low surrogate prefix
            uint low = ((uint)trailingSurrogate & SurrogateMask);

            // Add Surrogates and the Supplemental-Multilingual-Plane prefix
            uint value = Utf32.SupplementaryPlanePrefix + high + low;

            return value;
        }


        // From Utf32
        public static int FromUtf32(uint utf32, out char leadingSurrogate, out char trailingSurrogate)
        {
            int result = FromUtf32(utf32, out ushort leading, out ushort trailing);

            leadingSurrogate = (char)leading;

            trailingSurrogate = (char)trailing;

            return result;
        }

        public static int FromUtf32(uint utf32, out short leadingSurrogate, out short trailingSurrogate)
        {
            int result = FromUtf32(utf32, out ushort leading, out ushort trailing);

            leadingSurrogate = (short)leading;

            trailingSurrogate = (short)trailing;

            return result;
        }

        public static int FromUtf32(uint utf32, out ushort leadingSurrogate, out ushort trailingSurrogate)
        {
            if (utf32 < Utf32.SupplementaryPlanePrefix)
            {
                leadingSurrogate = (ushort)utf32;

                trailingSurrogate = 0;

                return 1;
            }
            else
            {
                SplitSurrogates(utf32, out leadingSurrogate, out trailingSurrogate);

                return 2;
            }
        }

        public static void SplitSurrogates(uint utf32, out ushort leadingSurrogate, out ushort trailingSurrogate)
        {
            // Get rid of the Supplemental-Multilingual-Plane prefix
            utf32 -= Utf32.SupplementaryPlanePrefix;

            // Shift high surrogate to right and add the high surrogate prefix
            leadingSurrogate = (ushort)(LeadingSurrogatePrefix | (utf32 >> LeadingSurrogateOffset));

            // Clear out the high surrogate and add the low surrogate prefix
            trailingSurrogate = (ushort)(TrailingSurrogatePrefix | (utf32 & SurrogateMask));
        }


        #region Errors

        private static EncodingException InvalidLeadingSurrogate(ushort value)
        {
            return new EncodingException(InvalidLeadingSurrogateMessage(value));
        }

        public static string InvalidLeadingSurrogateMessage(ushort value)
        {
            return string.Format("Invalid leading surrogate character. Must be in range (\\uD800-\\uDBFF), was \\u{0:X4}.", value);
        }


        private static EncodingException InvalidTrailingSurrogate(ushort value)
        {
            return new EncodingException(InvalidTrailingSurrogateMessage(value));
        }

        public static string InvalidTrailingSurrogateMessage(ushort value)
        {
            return string.Format("Invalid trailing surrogate character. Must be in range (\\uDC00-\\uDFFF), was \\u{0:X4}.", value);

        }

        private static InvalidCastException MissingLeadingSurrogate(ushort value)
        {
            throw new InvalidCastException(MissingLeadingSurrogateMessage(value));
        }

        public static string MissingLeadingSurrogateMessage(ushort value)
        {
            return string.Format("Missing leading surrogate; found trailing surrogate {0:X4}.", value);

        }

        private static InvalidCastException MissingTrailingSurrogate()
        {
            throw new InvalidCastException(MissingTrailingSurrogateMessage());
        }

        public static string MissingTrailingSurrogateMessage()
        {
            return string.Format("Missing trailing surrogate.");
        }


        private static EncodingException InvalidCharacter(ushort value)
        {
            return new EncodingException(InvalidCharacterMessage(value));
        }

        public static string InvalidCharacterMessage(ushort value)
        {
            return string.Format("Character cannot be a surrogate (in range \\uD800-\\uDFFF), was \\u{0:X4}", value);
        }

        #endregion


        public struct Encoder : IStepTransformer<uint, BitTwiddler>
        {
            bool reverse;

            public Encoder(ByteOrder endianness = ByteOrder.LittleEndian) => reverse = (endianness == ByteOrder.BigEndian);


            public (bool Complete, BitTwiddler Result) Process(uint data) => (true, Encode(data, reverse));

            private static BitTwiddler Encode(uint value, bool reverse)
            {
                int units = FromUtf32(value, out ushort leadingSurrogate, out ushort trailingSurrogate);

                BitTwiddler result;

                if (units == 1)
                    result = BitTwiddler.FromShort(leadingSurrogate);
                else
                    result = BitTwiddler.FromShorts(leadingSurrogate, trailingSurrogate);

                if (reverse)
                    result = result.ReverseBytesInShorts();

                return result;
            }

            public static BitTwiddler Encode(uint value, ByteOrder endianness = ByteOrder.LittleEndian)
            {
                return Encode(value, endianness == ByteOrder.BigEndian);
            }
        }

        public struct Decoder : IStepTransformer<BitTwiddler, uint>
        {
            bool reverse;

            public Decoder(ByteOrder endianness = ByteOrder.LittleEndian) => reverse = (endianness == ByteOrder.BigEndian);

            public (bool Complete, uint Result) Process(BitTwiddler data) => (true, Decode(data, reverse));

            private static uint Decode(BitTwiddler value, bool reverse)
            {
                if (reverse)
                    value = value.ReverseBytesInShorts();

                char leading = value.GetChar(0);

                if (IsLeadingSurrogate(leading))
                {
                    char trailing = value.GetChar(1);

                    if (IsTrailingSurrogate(trailing))
                        return CombineSurrogates(leading, trailing);
                    else
                        throw new EncodingException("Invalid trailing surrogate '0x" + Convert.ToString((ushort)trailing, 2) + "'.");
                }
                else
                {
                    return leading;
                }
            }


            public static uint Decode(BitTwiddler value, ByteOrder endianness = ByteOrder.LittleEndian)
            {
                return Decode(value, endianness == ByteOrder.BigEndian);
            }
        }

        public struct ByteDecoder : IStepTransformer<byte, uint>
        {
            bool isLittleEndian;

            ushort leadingSurrogate;

            ushort current;


            public ByteDecoder(ByteOrder endianness) : this()
            {
                this.isLittleEndian = endianness == ByteOrder.LittleEndian;
            }

            public (bool Complete, uint Result) Process(byte data) 
            {
                if (current == default(ushort))
                {
                    AddByte(data, !isLittleEndian);

                    return (false, 0);
                }
                else
                {
                    AddByte(data, isLittleEndian);

                    if (ProcessResult(out var result))
                        return (true, result);
                    else
                        return (false, 0);
                }
            }

            private void AddByte(ushort value, bool shift)
            {
                if (shift)
                    value <<= 8;

                current |= value;
            }

            private bool ProcessResult(out uint result)
            {
                if (leadingSurrogate == default(ushort))
                {
                    if (IsLeadingSurrogate(current))
                    {
                        leadingSurrogate = current;

                        result = default(ushort);

                        current = default(ushort);

                        return false;
                    }
                    else if (IsTrailingSurrogate(current))
                    {
                        current = default(ushort);

                        throw MissingLeadingSurrogate(current);
                    }
                    else
                    {
                        result = current;

                        current = default(ushort);

                        return true;
                    }
                }
                else
                {
                    if (IsTrailingSurrogate(current))
                    {
                        result = ToUtf32(leadingSurrogate, current);

                        current = leadingSurrogate = default(ushort);

                        return true;
                    }
                    else
                    {
                        current = leadingSurrogate = default(ushort);

                        throw InvalidTrailingSurrogate(current);
                    }
                }
            }
        }

        public struct CharDecoder : IStepTransformer<char, uint>
        {
            char leadingSurrogate;

            public (bool Complete, uint Result) Process(char data)
            {
                // Finished
                if (leadingSurrogate == default(char))
                {
                    if (IsLeadingSurrogate(data))
                    {
                        leadingSurrogate = data;

                        return (false, default(char));
                    }
                    else if (IsTrailingSurrogate(data))
                    {
                        throw MissingLeadingSurrogate(data);
                    }
                    else
                    {
                        var result = ToUtf32(data);

                        return (true, result);
                    }
                }
                else if (IsTrailingSurrogate(data))
                {
                    var result = CombineSurrogates(leadingSurrogate, data);

                    leadingSurrogate = default(char);

                    return (true, result);
                }
                else
                {
                    leadingSurrogate = default(char);

                    throw InvalidTrailingSurrogate(data);
                }
            }
        }
    }
}