using System;
using Veruthian.Dotnet.Library.Data.Operations;

namespace _Console
{
    class Program
    {
        private static readonly BooleanOperation<object> True = BooleanOperation<object>.True;

        private static readonly BooleanOperation<object> False = BooleanOperation<object>.False;

        private static InvertedOperation<object> Invert(IOperation<object> operation) => new InvertedOperation<object>(operation);

        private static OptionalOperation<object> Maybe(IOperation<object> operation) => new OptionalOperation<object>(operation);

        private static VariableSequentialOperation<object> AllOf(params IOperation<object>[] operations) => new VariableSequentialOperation<object>(SequenceType.AllOf, operations);

        private static VariableSequentialOperation<object> AnyOf(params IOperation<object>[] operations) => new VariableSequentialOperation<object>(SequenceType.AnyOf, operations);

        private static RepeatedOperation<object> Repeat(IOperation<object> operation, int min = 0, int max = 0) => new RepeatedOperation<object>(operation, min, max);


        static void Main(string[] args)
        {
            var item = AllOf().Add(True).Add(False).AddSelf();

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