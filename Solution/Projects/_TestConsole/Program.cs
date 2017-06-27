using System;
using System.Collections.Generic;
using System.IO;
using Soedeum.Dotnet.Library;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole
{
    class Program
    {        
        static void Main(string[] args)
        {
            var s = new CodeString('H', 'e', 'l', 'l', 'o');
            var t = new CodeString('H', 'e', 'l', 'l', 'o');
            Console.WriteLine(s == t);

            Pause();
        }

        // Comment
        static void Pause()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            System.Console.WriteLine();
        }
    }
}