using System;
using Veruthian.Library.Text.Lines;
using Veruthian.Library.Text.Chars.Extensions;
using System.Text;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
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