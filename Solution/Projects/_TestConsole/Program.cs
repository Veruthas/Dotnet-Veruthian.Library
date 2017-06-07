using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(CharSet.HexDigit);
            System.Console.WriteLine(CharSet.LetterOrDigit);
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
