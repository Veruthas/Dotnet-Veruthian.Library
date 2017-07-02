using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Text.Encodings
{
    public static class Utf32
    {
        public const uint SupplementaryPlanePrefix = 0x10000;

        public const uint MinCodePoint = 0x0;

        public const uint MaxCodePoint = 0x10FFFF;

        
        public const uint DisallowedCharacterStart = 0xFDD0;

        public const uint DisallowedCharacterEnd = 0xFDEF;


        public static bool IsValid(uint value) => !IsInvalid(value);

        
        public static bool IsInvalid(uint value) => IsDisallowed(value) || IsOutOfRange(value);

        public static bool IsAllowed(uint value) => !IsDisallowed(value);

        public static bool IsDisallowed(uint value)
        {
            return ((value & 0xFFFE) == 0xFFFE)
                    || (Utf16.IsSurrogate(value))
                    || (value >= DisallowedCharacterStart && value <= DisallowedCharacterEnd);
        }

        public static bool IsInRange(uint value) => (value <= MaxCodePoint);

        public static bool IsOutOfRange(uint value) => (value > MaxCodePoint);


        public static void VerifyIsValid(uint value)
        {
            VerifyInRange(value);

            VerifyAllowed(value);
        }

        public static void VerifyAllowed(uint value)
        {
            if (IsDisallowed(value))
                throw InvalidCodePoint(value);
        }

        public static void VerifyInRange(uint value)
        {
            if (IsOutOfRange(value))
                throw CodePointOutOfRange(value);
        }

        #region Errors

        private static InvalidCodePointException CodePointOutOfRange(uint value)
        {
            return new InvalidCodePointException(CodePointOutOfRangeMessage((int)value));
        }

        public static string CodePointOutOfRangeMessage(int value)
        {
            return string.Format("Codepoint cannot be greater than U+10FFFF, was U+{0:X4}", value);
        }


        private static InvalidCodePointException InvalidCodePoint(uint value)
        {
            return new InvalidCodePointException(InvalidCodePointMessage(value));
        }

        public static string InvalidCodePointMessage(uint value)
        {
            return string.Format("Invalid codepoint, was U+{0:X4}", value);
        }

        #endregion

        public struct ByteDecoder : ITransformer<byte, uint>
        {
            bool isLittleEndian;

            uint state;

            int bytesRemaining;


            public ByteDecoder(bool isLittleEndian) : this()
            {
                this.isLittleEndian = isLittleEndian;
            }

            public bool TryProcess(byte value, out uint result)
            {
                if (bytesRemaining == 0)
                    bytesRemaining = 3;
                else
                    bytesRemaining--;

                AddByte(value);

                if (bytesRemaining == 0)
                {
                    result = state;

                    state = 0;

                    return true;
                }
                else
                {
                    result = default(uint);

                    return false;
                }
            }

            private void AddByte(uint value)
            {
                if (isLittleEndian)
                    value <<= (8 * bytesRemaining);
                else
                    state <<= 8;


                state |= value;
            }
        }
    }
}