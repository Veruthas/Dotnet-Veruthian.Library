using System;
using System.Collections.Generic;
using System.IO;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            char a = (char)0x0045;

            char b = (char)0x00A2;

            string ab = "" + a + b;

            var c = new Utf8.Decoder();

            c.Process(0x45);

            var x = c.Result.GetValueOrDefault();

            c.Process(0xC2);
            c.Process(0xA2);

            var y = c.Result.GetValueOrDefault();

            string xy = "" + x + y;

            Console.WriteLine(ab == xy);

            Pause();
        }

        // Comment
        static void Pause()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            System.Console.WriteLine();
        }
    }
}