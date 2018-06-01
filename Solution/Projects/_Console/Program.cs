using System;
using System.Collections.Generic;
using Veruthian.Dotnet.Library.Data.Operations;
using Veruthian.Dotnet.Library.Data.Readers;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var op =
            SequentialOperation<IReader<char>>.AllOf(
                new ActionOperation<IReader<char>>((reader) => reader.Read() == 'H', "H"),
                new ActionOperation<IReader<char>>((reader) => reader.Read() == 'e', "e"),
                new ActionOperation<IReader<char>>((reader) => reader.Read() == 'l', "l"),
                new ActionOperation<IReader<char>>((reader) => reader.Read() == 'l', "l"),
                new ActionOperation<IReader<char>>((reader) => reader.Read() == 'o', "o"),
                new ActionOperation<IReader<char>>((reader) => reader.Read() == '\0', "$"),
                new ActionOperation<IReader<char>>((reader) => reader.IsEnd, "$")
                );

            bool result = op.Perform("Hello".GetSimpleReader());
            Console.WriteLine(result);

            result = op.Perform("Hellos".GetSimpleReader());
            Console.WriteLine(result);

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