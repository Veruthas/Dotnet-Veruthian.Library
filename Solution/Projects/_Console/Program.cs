using System;
using System.Diagnostics;
using Veruthian.Library.Numeric;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Number n = 0;

            ulong l = 0;

            Stopwatch s = new Stopwatch();

            s.Start();

            for (ulong i = 0; i < (ulong)uint.MaxValue + 1; i++)
                l++;

            s.Stop();

            Console.WriteLine(s.Elapsed);


            s.Reset();

            s.Start();

            for (ulong i = 0; i < (ulong)uint.MaxValue + 1; i++)
                n++;

            s.Stop();

            Console.WriteLine(s.Elapsed);

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
