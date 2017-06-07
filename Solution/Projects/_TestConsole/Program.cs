using System;
using System.Collections.Generic;

namespace _TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TestCollections.Test();            
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
