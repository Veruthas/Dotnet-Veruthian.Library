using System;
using System.Collections.Generic;
using System.IO;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            char c = "𐐷"[0];
            char d = "𐐷"[1];

            string s = "𐐷";

            Console.WriteLine(s == "" + c );

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