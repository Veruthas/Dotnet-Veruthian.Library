using System;
using System.Text;
using Veruthian.Dotnet.Library.Data;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 2; i <= 10; i++)
                Console.WriteLine(MakeType(i));
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