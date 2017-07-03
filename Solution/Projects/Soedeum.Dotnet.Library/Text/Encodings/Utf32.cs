using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Text.Encodings
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


        #region Errors

        private static InvalidCodePointException CodePointOutOfRange(int value)
        {
            return new InvalidCodePointException(CodePointOutOfRangeMessage(value));
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