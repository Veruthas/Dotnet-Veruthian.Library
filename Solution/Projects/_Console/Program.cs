using System;
using Veruthian.Library.Collections.Extensions;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = "Helloworld....".ToCharArray();
            a.ForceSpace(5, 2, 10);
            a[5] = ',';
            a[6] = ' ';
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
