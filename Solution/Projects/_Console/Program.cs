using System;
using Veruthian.Library.Text.Chars;
using Veruthian.Library.Text.Chars.Extensions;
using Veruthian.Library.Text.Runes;
using Veruthian.Library.Text.Runes.Extensions;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            RuneLineTable lines;

            foreach (var r in "Hello\rworld\r\nLevi".ToRuneString().ProcessLines(out lines))
            {
                Console.WriteLine(r.ToPrintableString());
            }
            Console.WriteLine();

            foreach (var rs in lines.ExtractLines("Hello\rworld\r\nLevi", true))
            {
                Console.WriteLine(rs.ToPrintableString());
            }

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