namespace Veruthian.Dotnet.Library.Numeric
{
    public static class NumericUtility
    {
        public static (uint result, uint carryOut) Add(uint left, uint right, uint carryIn = 0)
        {
            ulong result = (ulong)left + (ulong)right + (ulong)carryIn;

            return ((uint)result, (uint)(result >> 32));
        }
    }
}