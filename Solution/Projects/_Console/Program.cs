using System;
using Veruthian.Library.Text.Chars;
using Veruthian.Library.Text.Chars.Extensions;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = "Hello\r\nMy\nName\ris Levi";

            var l = new CharLineTable();

            // for (int i = s.Length - 1; i >= 0; i--)
            //     l.Prepend(s[i]);

            // l.Prepend(s);

            // for (int i = 0; i < s.Length; i++)
            //     l.Append(s[i]);

            l.Append(s);

            foreach (var line in l.ExtractLines(s))
                Console.WriteLine($"'{line.AsPrintable()}'");

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