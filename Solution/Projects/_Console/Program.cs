using System;
using Veruthian.Dotnet.Library.Collections;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IPool<string, int> pool = new DataPool<string, int>();

            pool.Resolve("Hello", 10);

            pool.Resolve("Goodbye", 10);

            while (true)
            {
                var r = GetPair();

                if (!r.success)
                    break;

                var a = pool.Resolve(r.pair.key, r.pair.value);
            }

            Console.WriteLine(pool);

            Pause();
        }

        private static (bool success, (string key, int value) pair) GetPair()
        {
            Console.Write("Enter a key: ");

            var key = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(key))
            
                return (false, default((string, int)));
            

            Console.Write("Enter a number: ");

            var value = Console.ReadLine();

            int.TryParse(value, out var num);

            return (true, (key, num));
        }

        static void Pause()
        {
            Console.Write("Press any key to continue...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

            Console.WriteLine();
        }
    }
}