using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Veruthian.Dotnet.Library;
using Veruthian.Dotnet.Library.Data;
using Veruthian.Dotnet.Library.Data.Readers;
using Veruthian.Dotnet.Library.Data.Ranges;
using Veruthian.Dotnet.Library.Numeric;
using Veruthian.Dotnet.Library.Text.Code;
using Veruthian.Dotnet.Library.Text.Code.Encodings;

namespace _TestConsole
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
            Console.ReadKey(true);
            System.Console.WriteLine();
        }
    }
}