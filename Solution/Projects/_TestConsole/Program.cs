using System;

namespace _TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Pause();
        }

        static void Pause()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            System.Console.WriteLine();
        }
    }
}
