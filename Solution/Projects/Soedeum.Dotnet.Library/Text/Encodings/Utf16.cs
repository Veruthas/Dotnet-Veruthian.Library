namespace Soedeum.Dotnet.Library.Text
{
    public class Utf16
    {
        public static readonly ushort HighSurrogateMin = 0xD800;

        public static readonly ushort HighSurrogateMax = 0xDBFF;


        public static readonly ushort LowSurrogateMin = 0xDC00;

        public static readonly ushort LowSurrogateMax = 0xDFFF;


        public static readonly ushort SurrogateMin = HighSurrogateMin;

        public static readonly ushort SurrogateMax = LowSurrogateMax;


        // 0000-0011 1111-1111
        public const ushort SurrogateMask = 0x03FF;

        // 110110 wwwwxxxxxx    =>  [1]: 1101-10ww [0]: wwxx-xxxx
        public const ushort HighSurrogatePrefix = 0xD800;

        // 110111 yyyyyyyyyy    =>  [1]: 1101-11yy [0]: yyyy-yyyy
        public const ushort LowSurrogatePrefix = 0xDC00;

        
        public static bool IsHighSurrogate(uint value) => (value >= HighSurrogateMin) && (value <= HighSurrogateMax);

        public static bool IsLowSurrogate(uint value) => (value >= LowSurrogateMin) && (value <= LowSurrogateMax);

        public static bool IsSurrogate(uint value) => (value >= SurrogateMin) && (value <= SurrogateMax);




        public const int HighSurrogateOffset = 10;


        public static uint CombineSurrogates(ushort highSurrogate, ushort lowSurrogate)
        {
            // Remove high surrogate prefix, and shift left
            uint high = ((uint)highSurrogate & SurrogateMask) << HighSurrogateOffset;

            // Remove low surrogate prefix
            uint low = ((uint)lowSurrogate & SurrogateMask);

            // Add Surrogates and the Supplemental-Multilingual-Plane prefix
            uint value = Utf32.SupplementaryPlanePrefix + high + low;

            return value;
        }

        public static void SplitSurrogates(uint utf32, out ushort highSurrogate, out ushort lowSurrogate)
        {
            // Get rid of the Supplemental-Multilingual-Plane prefix
            utf32 -= Utf32.SupplementaryPlanePrefix;

            // Shift high surrogate to right and add the high surrogate prefix
            highSurrogate = (ushort)(HighSurrogatePrefix | (utf32 >> HighSurrogateOffset));

            // Clear out the high surrogate and add the low surrogate prefix
            lowSurrogate = (ushort)(LowSurrogatePrefix | (utf32 & SurrogateMask));
        }
    }
}