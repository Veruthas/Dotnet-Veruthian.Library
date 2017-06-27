using System;

namespace Soedeum.Dotnet.Library.Text
{
    public static class Utf32
    {
        public const uint SupplementaryPlanePrefix = 0x10000;

        public const uint MaxCodePoint = 0x10FFFF;

        public static bool IsValid(uint value) => !IsDisallowed(value);

        public static bool IsInvalid(uint value) => IsDisallowed(value) || IsOutOfRange(value);

        public static bool IsDisallowed(uint value)
        {
            return ((value & 0xFFFE) == 0xFFFE)
                    || (Utf16.IsSurrogate(value))
                    || (value >= 0xFDD0 && value <= 0xFDEF);
        }

        public static bool IsOutOfRange(uint value) => (value > MaxCodePoint);

        public static void Verify(uint value)
        {
            if (IsOutOfRange(value))
                throw CodePointOutOfRange(value);

            else if (IsDisallowed(value))
                throw InvalidCodePoint(value);

        }

        #region Errors

        private static InvalidCodePointException CodePointOutOfRange(uint value)
        {
            return new InvalidCodePointException(CodePointOutOfRangeMessage(value));
        }

        private static string CodePointOutOfRangeMessage(uint value)
        {
            return string.Format("Codepoint cannot be greater than U+10FFFF, was U+{0:X4}", value);
        }


        private static InvalidCodePointException InvalidCodePoint(uint value)
        {
            return new InvalidCodePointException(InvalidCodePointMessage(value));
        }

        private static string InvalidCodePointMessage(uint value)
        {
            return string.Format("Invalid codepoint, was U+{0:X4}", value);
        }

        #endregion

        public class ByteDecoder : ITransformer<byte, CodePoint?>
        {
            public ByteDecoder(bool littleEndian = false)
            {
                
            }

            public CodePoint? Result => throw new NotImplementedException();

            public bool Process(byte value)
            {
                throw new NotImplementedException();
            }
        }
    }
}