using System;
using System.Collections.Generic;
using System.IO;
using _TestConsole.Numb;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var set = CharacterSet.FromList('A', 'D', 'E', 'F', 'Z', 'M', 'N', 'O');
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