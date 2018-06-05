using System;
using Veruthian.Dotnet.Library.Data.Operations;

namespace _Console
{
    class Program
    {
        private static readonly IOperation<object> True = BooleanOperation<object>.True;
        private static readonly IOperation<object> False = BooleanOperation<object>.False;

        private static IOperation<object> Invert(IOperation<object> operation) => new InvertedOperation<object>(operation);

        private static IOperation<object> Maybe(IOperation<object> operation) => new OptionalOperation<object>(operation);

        private static IOperation<object> AllOf(params IOperation<object>[] operations) => new VariableSequentialOperation<object>(SequenceType.AllOf, operations);

        private static IOperation<object> AnyOf(params IOperation<object>[] operations) => new FixedSequentialOperation<object>(SequenceType.AnyOf, operations);

        private static IOperation<object> Repeat(IOperation<object> operation, int min = 0, int max = 0) => new RepeatedOperation<object>(operation, min, max);


        static void Main(string[] args)
        {
            var item = AllOf(True, True, False, AnyOf(Maybe(True), Repeat(True, 0, 5)));

            var flat = item.Flatten();

            for (int i = 0; i < flat.Length; i++)
            {
                var f = flat[i];

                Console.WriteLine($"{i}: {f}");
            }

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