using System;

namespace Soedeum.Dotnet.Library.Text.Encodings
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

        private static InvalidCodePointException InvalidLeadingSurrogate(ushort value)
        {
            return new InvalidCodePointException(InvalidLeadingSurrogateMessage(value));
        }

        private static string InvalidLeadingSurrogateMessage(ushort value)
        {
            return string.Format("Invalid leading surrogate character. Must be in range (\\uD800-\\uDBFF), was \\u{0:X4}.", value);
        }


        private static InvalidCodePointException InvalidTrailingSurrogate(ushort value)
        {
            return new InvalidCodePointException(InvalidTrailingSurrogateMessage(value));
        }

        private static string InvalidTrailingSurrogateMessage(ushort value)
        {
            return string.Format("Invalid trailing surrogate character. Must be in range (\\uDC00-\\uDFFF), was \\u{0:X4}.", value);

        }

        private static InvalidCastException MissingLeadingSurrogate(ushort value)
        {
            throw new InvalidCastException(MissingLeadingSurrogateMessage(value));
        }

        private static string MissingLeadingSurrogateMessage(ushort value)
        {
            return string.Format("Missing leading surrogate; found trailing surrogate {0:X4}.", value);

        }

        private static InvalidCodePointException InvalidCharacter(ushort value)
        {
            return new InvalidCodePointException(InvalidCharacterMessage(value));
        }

        private static string InvalidCharacterMessage(ushort value)
        {
            return string.Format("Character cannot be a surrogate (in range \\uD800-\\uDFFF), was \\u{0:X4}", value);
        }

        #endregion




        public struct ByteDecoder : ITransformer<byte, uint>
        {
            bool isLittleEndian;

            ushort leadingSurrogate;

            ushort current;


            public ByteDecoder(bool isLittleEndian) : this()
            {
                this.isLittleEndian = isLittleEndian;
            }

            public bool TryProcess(byte value, out uint result)
            {
                if (current == default(ushort))
                {
                    AddByte(value, !isLittleEndian);

                    result = default(uint);

                    return false;
                }
                else
                {
                    AddByte(value, isLittleEndian);

                    return ProcessResult(out result);
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

        public struct CharDecoder : ITransformer<char, uint>
        {
            char leadingSurrogate;

            public bool TryProcess(char value, out uint result)
            {
                // Finished
                if (leadingSurrogate == default(char))
                {
                    if (IsLeadingSurrogate(value))
                    {
                        leadingSurrogate = value;

                        result = default(uint);

                        return false;
                    }
                    else if (IsTrailingSurrogate(value))
                    {
                        throw MissingLeadingSurrogate(value);
                    }
                    else
                    {
                        result = ToUtf32(value);

                        return true;
                    }
                }
                else if (IsTrailingSurrogate(value))
                {
                    result = CombineSurrogates(leadingSurrogate, value);

                    leadingSurrogate = default(char);

                    return true;
                }
                else
                {
                    leadingSurrogate = default(char);

                    throw InvalidTrailingSurrogate(value);
                }
            }
        }
    }
}