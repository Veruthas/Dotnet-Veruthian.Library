using System;
using Veruthian.Library.Text;
using Veruthian.Library.Text.Runes;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            RuneString value = new RuneString("Hello\nWorld\rMy Name is\r\nLevi");

            LineIndexTable lines = new LineIndexTable();

            lines.MoveThrough('\0', value + '\0');


            foreach (var line in lines.ExtractLines(value))
            {
                string result = "";

                foreach (var rune in line)
                {
                    result += rune.ToPrintableString();
                }

                Console.WriteLine($"'{result}'");
            }

            Console.WriteLine();

            foreach (var line in lines.ExtractLines(value, false))
            {
                string result = "";

                foreach (var rune in line)
                {
                    result += rune.ToPrintableString();
                }

                Console.WriteLine($"'{result}'");
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