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
            var builder = CharLineTester.Build("AppendMultiple", "None");
            builder.WriteBuilder();

            builder = CharLineTester.Build("AppendMultiple", "Lf");
            builder.WriteBuilder();
            
            builder = CharLineTester.Build("AppendMultiple", "Cr");
            builder.WriteBuilder();
            
            builder = CharLineTester.Build("AppendMultiple", "CrLf");
            builder.WriteBuilder();

            Pause();
        }

        private static void TestEnding()
        {
            var ending = LineEnding.None;

            var builder = new CharLineBuilder(ending);

            builder.Append("Hello\rWorld\nMy\r\n");
            builder.WriteBuilder();

            builder.Insert(15, "name is Veruthas!\n");
            builder.WriteBuilder();

            builder.Insert(11, "!!\r");
            builder.WriteBuilder();

            builder.Insert(6, "\n, ");
            builder.WriteBuilder();

        }

        static void Pause()
        {
            Console.Write("Press any key to continue...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

            Console.WriteLine();
        }
    }
}