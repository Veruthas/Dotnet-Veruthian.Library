using System;
using Veruthian.Library.Readers.Extensions;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            TestParsers.TestParser();

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
