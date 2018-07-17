using System;
using Veruthian.Dotnet.Library.Collections;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new DataArray<int>(1, 2, 3);

            var s = new IndexSegment<int>(a, 1, a.Count - 1, -10);

            foreach (var k in ((ILookup<int, int>)s).Keys)
                Console.WriteLine($"[{k}]: {s[k]}");

            Console.WriteLine();

            foreach (var p in s.Pairs)
                Console.WriteLine(p);

            Console.WriteLine();
            Console.WriteLine(s);

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