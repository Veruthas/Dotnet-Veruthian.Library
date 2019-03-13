using System;
using Veruthian.Library.Text;
using Veruthian.Library.Text.Runes;
using Veruthian.Library.Text.Runes.Extensions;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            RuneString value = new RuneString("Hello\nWorld\rMy Name is\r\nLevi");

            LineIndexTable lines = new LineIndexTable();

            foreach(var rune in value.ProcessLines(out lines))
                Console.Write(rune.ToPrintableString());

            Console.WriteLine("\nRESULTS\n");

            // foreach (var line in lines.ExtractLines(value))
            // {
            //     string result = "";

            //     foreach (var rune in line)
            //     {
            //         result += rune.ToPrintableString();
            //     }

            //     Console.WriteLine($"'{result}'");
            // }

            // Console.WriteLine();

            // foreach (var line in lines.ExtractLines(value, false))
            // {
            //     string result = "";

            //     foreach (var rune in line)
            //     {
            //         result += rune.ToPrintableString();
            //     }

            //     Console.WriteLine($"'{result}'");
            // }


            for (int i = 0; i < value.Length; i++)
            {
                var location = lines.GetTextLocation(i);
                Console.WriteLine($"{i}: '{value[i].ToPrintableString()}' @ {location}");
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