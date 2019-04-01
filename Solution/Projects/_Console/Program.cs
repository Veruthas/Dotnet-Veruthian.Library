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
            var ending = LineEnding.CrLf;

            CharLineBuilder builder = new CharLineBuilder(ending);


            builder.Append("Hello\rWorld\nMy\r\n");

            foreach (var line in builder.Value.GetLineData(ending))
                Console.WriteLine((line.LineNumber, line.Ending, $"'{line.Value.ToPrintableString()}'", line.Value.Length));

            Console.WriteLine("-");

            foreach (var line in builder.Lines.Lines)
                Console.WriteLine(line);

            Console.WriteLine();


            builder.Insert(15, "name is Veruthas!");

            foreach (var line in builder.Value.GetLineData(ending))
                Console.WriteLine((line.LineNumber, line.Ending, $"'{line.Value.ToPrintableString()}'", line.Value.Length));

            Console.WriteLine("-");

            foreach (var line in builder.Lines.Lines)
                Console.WriteLine(line);

            Console.WriteLine();


            builder.Insert(11, "!!\r");

            foreach (var line in builder.Value.GetLineData(ending))
                Console.WriteLine((line.LineNumber, line.Ending, $"'{line.Value.ToPrintableString()}'", line.Value.Length));

            Console.WriteLine("-");

            foreach (var line in builder.Lines.Lines)
                Console.WriteLine(line);

            Console.WriteLine();


            builder.Insert(6, "\n, ");

            foreach (var line in builder.Value.GetLineData(ending))
                Console.WriteLine((line.LineNumber, line.Ending, $"'{line.Value.ToPrintableString()}'", line.Value.Length));

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