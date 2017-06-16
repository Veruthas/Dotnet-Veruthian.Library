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
            TestText.Test();

            var s = new Dictionary<char, object>();
            foreach(char c in CharacterSet.All)
                s.Add(c, (int)c);

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