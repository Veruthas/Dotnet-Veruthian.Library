using System;

namespace _Console.Experiments
{
    public static class Powers
    {
        public static (ulong result, int operations) Power(uint value, uint exponent)
        {
            ulong result = 1;

            ulong last = value;

            int operations = 0;

            int pow2 = 1;

            Console.Write($"{value}^{exponent} = {value}^0");

            while (exponent != 0)
            {
                if ((exponent & 1) == 1)
                {
                    Console.Write($" * {value}^{pow2}");

                    result *= last;

                    operations++;
                }

                exponent >>= 1;

                last *= last;

                pow2 *= 2;
            }

            Console.WriteLine();
            
            return (result, operations);
        }
    }
}