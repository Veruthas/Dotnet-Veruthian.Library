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

            var builder = new CharLineBuilder(ending);

            builder.Append("Hello\rWorld\nMy\r\n");
            WriteBuilder(builder);

            builder.Insert(15, "name is Veruthas!\n");
            WriteBuilder(builder);

            builder.Insert(11, "!!\r");
            WriteBuilder(builder);

            builder.Insert(6, "\n, ");
            WriteBuilder(builder);


            Pause();
        }

        private static void WriteBuilder(CharLineBuilder builder)
        {
            Console.WriteLine($"'{builder.Value.ToString().ToPrintableString()}'");

            Console.WriteLine("Split");

            foreach (var line in builder.Value.ToString().GetLineData(builder.Ending))
            {
                var segment = line.Segment.ToTupleString();

                Console.WriteLine("{0}{1}-> '{2}'", segment, new string(' ', 20 - segment.Length), line.Value.ToPrintableString());
            }

            Console.WriteLine("-------");

            Console.WriteLine("Table");
            foreach (var line in builder.Lines.Lines)
            {
                Console.WriteLine(line.ToTupleString());
            }

            Console.WriteLine();
        }

        static void Pause()
        {
            Console.Write("Press any key to continue...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

            Console.WriteLine();
        }
    }
}