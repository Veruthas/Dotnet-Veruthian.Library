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

            var table = new DataTable<int, int>();

            table.Insert(-10, 1);

            table.Insert(-9, 2);

            table.Insert(-8, 3);

            foreach (var p in table.Pairs)
                Console.WriteLine(p);

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