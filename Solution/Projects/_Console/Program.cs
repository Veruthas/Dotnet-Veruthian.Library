using System;
using Veruthian.Library.Text.Chars;
using Veruthian.Library.Text.Chars.Extensions;
using Veruthian.Library.Text.Lines;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = "Hello\r\nMy\nName\ris Levi";

            var l = new CharLineTable(LineEnding.CrLf);

            // for (int i = s.Length - 1; i >= 0; i--)
            //     l.Prepend(s[i]);

            // l.Prepend(s);

            // for (int i = 0; i < s.Length; i++)
            //     l.Append(s[i]);

            l.Append(s);


            // s = s.Insert(15, "\n");
            // Console.WriteLine(s.ToPrintableString());
            // l.Insert(15, '\n');

            s = s.Insert(9, "\r");
            Console.WriteLine(s.ToPrintableString());
            l.Insert(9, '\r');

            foreach (var line in l.ExtractLines(s))
                Console.WriteLine($"'{line.ToPrintableString()}'");

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