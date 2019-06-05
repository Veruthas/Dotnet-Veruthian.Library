using System;
using System.Linq;
using Veruthian.Library.Numeric;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = Number.FromBytes(new byte[] { 0xEF, 0xCD, 0xAB, 0x89 }, 3);

            var b = a.ToArray();
            
            Pause();
        }

        public static void Pause()
        {
            Console.Write("Press any key to continue...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

            Console.WriteLine();
        }
    }
}
