using System;
using Veruthian.Library.Numeric;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Number a = Number.Parse("80-00-00-00", Number.HexUpper);
            Number b = uint.MaxValue;

            var c = a + b;
            var d = b - a;
            var e = a - b;

            var e2 = d + a;

            var f = a * b;

            var fs = f.ToString();

            var f2 = f.Previous;

            var g = f / b;

            var h = a % b;

            Pause();
        }

        public static void Pause()
        {
            Console.Write("Press any key to continue...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

            Console.WriteLine();
        }
    }
}
