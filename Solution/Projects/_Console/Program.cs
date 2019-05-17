using System;
using Veruthian.Library.Numeric;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Number a = ushort.MaxValue;

            Number b = a + a + a;

            Number c = a * 3;

            Number d = c / 256;
            
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
