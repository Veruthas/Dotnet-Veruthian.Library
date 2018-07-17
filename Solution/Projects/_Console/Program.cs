using System;
using Veruthian.Dotnet.Library.Collections;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new DataArray<int>(1, 2, 3);


            foreach (var p in a.Pairs)
                Console.WriteLine(p);

            Console.WriteLine(a);

            Pause();
        }

        static void Pause()
        {
            Console.Write("Press any key to continue...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

            Console.WriteLine();
        }
    }
}