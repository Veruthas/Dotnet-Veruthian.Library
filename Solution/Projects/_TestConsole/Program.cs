using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateEndItem<int> item = (i) => i;

            Func<int, int> s = (i) => i + 1;

            Pause();
        }

        static void Pause()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            System.Console.WriteLine();
        }
    }
}