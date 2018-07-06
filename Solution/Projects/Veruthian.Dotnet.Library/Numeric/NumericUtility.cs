namespace Veruthian.Dotnet.Library.Numeric
{
    public static class NumericUtility
    {
        public static (uint result, uint carryOut) Add(uint left, uint right, uint carryIn = 0)
        {
            ulong result = (ulong)left + (ulong)right + (ulong)carryIn;

            return ((uint)result, (uint)(result >> 32));
        }

        public static uint[] Add(uint value, uint[] values)
        {
            int oldSize = values.Length;

            bool overflow = (values[oldSize - 1] == uint.MaxValue);

            var newValues = new uint[overflow ? oldSize : oldSize + 1];

            uint carry = value;

            for (int i = 0; i < oldSize; i++)
                (newValues[i], carry) = NumericUtility.Add(values[i], carry);

            if (overflow)
                newValues[oldSize] = carry;

            return newValues;
        }

        public static uint[] Add(uint[] left, uint[] right)
        {
            return new uint[0];
        }

    }
}