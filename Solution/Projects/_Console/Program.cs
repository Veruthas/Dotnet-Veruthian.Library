using System;
using System.Diagnostics;
using Veruthian.Dotnet.Library.Collections;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {

            float x = 10.23f;

            x++;
            Console.WriteLine(x);

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