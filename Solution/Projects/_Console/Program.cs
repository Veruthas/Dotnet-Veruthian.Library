using System;
using Veruthian.Library.Text.Chars.Extensions;
using Veruthian.Library.Text.Lines;
using Veruthian.Library.Text.Lines.Test;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var ending = LineEnding.None;

            CharLineBuilder builder = new CharLineBuilder(ending);


            builder.Append("Hello\rWorld\nMy\r\n");

            Console.WriteLine(builder.Value.ToString().ToPrintableString() + "\n");

            var position = 0;
            foreach (var line in builder.Value.GetLineData(ending))
            {
                var text = (line.LineNumber, position, line.Value.Length, line.Ending).ToString();
                Console.WriteLine($"{text}{new string(' ', 22 - text.Length)}-> '{line.Value.ToPrintableString()}'");
                position += line.Value.Length;
            }

            Console.WriteLine("-");

            foreach (var line in builder.Lines.Lines)
                Console.WriteLine(line);

            Console.WriteLine();


            builder.Insert(15, "name is Veruthas!\n");

            Console.WriteLine(builder.Value.ToString().ToPrintableString() + "\n");

            position = 0;
            foreach (var line in builder.Value.GetLineData(ending))
            {
                var text = (line.LineNumber, position, line.Value.Length, line.Ending).ToString();
                Console.WriteLine($"{text}{new string(' ', 22 - text.Length)}-> '{line.Value.ToPrintableString()}'");
                position += line.Value.Length;
            }

            Console.WriteLine("-");

            foreach (var line in builder.Lines.Lines)
                Console.WriteLine(line);

            Console.WriteLine();


            builder.Insert(11, "!!\r");

            Console.WriteLine(builder.Value.ToString().ToPrintableString() + "\n");

            position = 0;
            foreach (var line in builder.Value.GetLineData(ending))
            {
                var text = (line.LineNumber, position, line.Value.Length, line.Ending).ToString();
                Console.WriteLine($"{text}{new string(' ', 22 - text.Length)}-> '{line.Value.ToPrintableString()}'");
                position += line.Value.Length;
            }

            Console.WriteLine("-");

            foreach (var line in builder.Lines.Lines)
                Console.WriteLine(line);

            Console.WriteLine();


            builder.Insert(6, "\n, ");

            Console.WriteLine(builder.Value.ToString().ToPrintableString() + "\n");

            position = 0;
            foreach (var line in builder.Value.GetLineData(ending))
            {
                var text = (line.LineNumber, position, line.Value.Length, line.Ending).ToString();
                Console.WriteLine($"{text}{new string(' ', 22 - text.Length)}-> '{line.Value.ToPrintableString()}'");
                position += line.Value.Length;
            }

            Console.WriteLine("-");

            foreach (var line in builder.Lines.Lines)
                Console.WriteLine(line);

            Console.WriteLine();


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