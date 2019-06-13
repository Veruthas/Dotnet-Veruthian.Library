using System;
using Veruthian.Library.Patterns;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
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
