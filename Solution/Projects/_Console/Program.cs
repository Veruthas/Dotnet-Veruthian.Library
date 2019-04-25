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
            Tests();

            Pause();
        }


        private static void Tests()
        {
            var builder = CharLineTester.Build("RemoveSimpleEnd", "None");
            builder.WriteBuilder();

            builder = CharLineTester.Build("RemoveSimpleEnd", "Lf");
            builder.WriteBuilder();

            builder = CharLineTester.Build("RemoveSimpleEnd", "Cr");
            builder.WriteBuilder();

            builder = CharLineTester.Build("RemoveSimpleEnd", "CrLf");
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