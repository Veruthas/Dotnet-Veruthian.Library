using System;
using System.Linq;
using Veruthian.Dotnet.Library.Data.Operations;
using Veruthian.Dotnet.Library.Data.Patterns;
using Veruthian.Dotnet.Library.Data.Ranges;
using Veruthian.Dotnet.Library.Data.Readers;
using Veruthian.Dotnet.Library.Text.Code;

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


        private class Marker<TReader> : NestedOperation<TReader>
            where TReader : ISpeculativeReader<CodePoint>
        {
            public Marker(IOperation<TReader> operation) : base(operation)
            {
            }

            public override string Description => "Hello";

            protected override bool DoAction(TReader state, IOperationTracer<TReader> tracer = null)
            {
                state.Mark();

                var success = Operation.Perform(state, tracer);

                if (success)
                {
                    var str = new CodeString(state.PeekFromMark(0, state.Position - state.MarkPosition));

                    Console.WriteLine(str.ToString().ToUpper());

                    state.Commit();

                    return true;
                }

                state.Commit();

                return false;
            }
        }

        static void Main(string[] args)
        {
            var letterSet = RangeSet<CodePoint>.Union(RangeSet<CodePoint>.Range('A', 'Z'), RangeSet<CodePoint>.Range('a', 'z'));

            var whitespaceSet = RangeSet<CodePoint>.List(' ', '\t', '.');


            
            var letters = new MatchInSetOperation<CodePoint, ISpeculativeReader<CodePoint>>(letterSet);

            var repeatletters = new RepeatedOperation<ISpeculativeReader<CodePoint>>(letters, 1);

            var whitespace = new MatchInSetOperation<CodePoint, ISpeculativeReader<CodePoint>>(whitespaceSet);

            var matcher = new Marker<ISpeculativeReader<CodePoint>>(repeatletters);


            

            var op = whitespace.Repeat().And(matcher).And(whitespace.Repeat()).Repeat();

            op.Perform("Hello world\t\t my . .name is Levi Minkoff".ToCodePoints().GetSpeculativeReader());


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