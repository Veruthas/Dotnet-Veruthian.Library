namespace Soedeum.Dotnet.Library.Text
{
    public class Utf32
    {
        public const uint SupplementaryPlanePrefix = 0x10000;


        public static bool IsValid(uint value) => IsInvalid(value);

        public static bool IsInvalid(uint value)
        {
            return ((value & 0xFFFE) == 0xFFFE)
                    || (Utf16.IsSurrogate(value))
                    || (value >= 0xFDD0 && value <= 0xFDEF);
        }
    }
}