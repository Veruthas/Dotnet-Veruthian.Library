using System;
using System.Diagnostics;
using Veruthian.Dotnet.Library.Collections;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            B b = new B(10);
            Pause();
        }

        static void Pause()
        {
            Console.Write("Press any key to continue...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

            Console.WriteLine();
        }

        [DebuggerDisplay("{ToString(),nq}")]
        class B 
        {
            int d;

            public B(int d)
            {
                this.d = d;
            }
            

            public override string ToString() => d.ToString();
        }
    }
}