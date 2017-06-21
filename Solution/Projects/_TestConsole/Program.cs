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
            //TestText.Test();            
            var set = CharSet.LetterOrDigit;

            foreach (char c in set)
                Console.WriteLine("'{0}'", c);

            var ranges = new CharRange[] { };

            var reduced = CharRange.Reduce(ranges);

            var nothing = CharRange.ComplementOrderedSet(reduced);

            var all = CharRange.ComplementOrderedSet(nothing);
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