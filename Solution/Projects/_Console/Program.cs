using System;
using Veruthian.Library.Numeric;
using Veruthian.Library.Operations;
using Veruthian.Library.Operations.Extensions;
using Veruthian.Library.Operations.Analyzers;
using Veruthian.Library.Processing;
using Veruthian.Library.Readers;
using Veruthian.Library.Readers.Extensions;
using Veruthian.Library.Text.Runes;
using Veruthian.Library.Types;
using Veruthian.Library.Types.Extensions;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            TestParser();

            Pause();
        }

        static void Pause()
        {
            Console.Write("Press any key to continue...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

            Console.WriteLine();
        }

        private static void TestParser()
        {
            var data = GetBuilder(TypeSet.Create<ISpeculativeReader<Rune>, ISpeculative>());

            var b = data.Builder;

            var rule = data.Rules;

            var tracer = data.tracer;


            // rule File = Whitespace Items unless Any;
            rule["File"] = b.Sequence(rule["Whitespace"], rule["Items"], b.Unless(b.MatchSet(RuneSet.Complete)));

            // Items = {Item};
            rule["Items"] = b.Repeat(rule["Item"]);

            // Item = Number or Word;
            rule["Item"] = b.Choice(rule["Number"], rule["Word"]);

            // rule Number = (at least 1 <'0' to '9'>) Whitespace;
            rule["Number"] = b.Sequence(b.AtLeast(1, b.MatchSet(RuneSet.Digit)), rule["Whitespace"]);

            // rule Word = (at least 1 <'A' to 'Z' + 'a' to 'z'>) Whitespace;
            rule["Word"] = b.Sequence(b.AtLeast(1, b.MatchSet(RuneSet.Letter)), rule["Whitespace"]);

            // rule Whitespace = {<' ' + '\t'>};
            rule["Whitespace"] = b.Repeat(b.MatchSet(RuneSet.TabOrSpace));


            // var items = rule["File"].Flatten();

            // foreach(var item in items)
            //     Console.WriteLine(item);

            Console.WriteLine(rule.FormatRule("File"));

            Console.WriteLine(rule.FormatRule("Items"));

            Console.WriteLine(rule.FormatRule("Item"));

            Console.WriteLine(rule.FormatRule("Number"));

            Console.WriteLine(rule.FormatRule("Word"));

            Console.WriteLine(rule.FormatRule("Whitespace").Replace("\t", "\\t"));


            Pause();

            Console.WriteLine();

            while (true)
            {
                Console.Write("Parse String> ");
                var s = (RuneString)Console.ReadLine();

                var r = s.GetSpeculativeReader();

                bool result = rule.Perform("File", r.GroupWith((ISpeculative)r), tracer);

                Console.WriteLine($"Result: {(result ? "Success" : "Failed")}");

                Console.WriteLine(r.Position);
                
                Console.WriteLine();
            }

        }


        static (AnalyzerBuilder<TState, ISpeculativeReader<Rune>, Rune> Builder, RuleTable<TState> Rules, Tracer<TState> tracer) GetBuilder<TState>(TState mockup)
            where TState : Has<ISpeculative>, Has<ISpeculativeReader<Rune>>
        {
            return (new AnalyzerBuilder<TState, ISpeculativeReader<Rune>, Rune>(), new RuleTable<TState>(), new Tracer<TState>());
        }

        public class Tracer<TState> : ITracer<TState>
        {
            int increment = 0;

            public void OnStart(IOperation<TState> operation, TState state, out bool? handled)
            {
                Console.WriteLine($"{new string('|', increment)}Starting {{{operation}}}");

                handled = null;

                increment++;
            }

            public void OnFinish(IOperation<TState> operation, TState state, bool success)
            {
                increment--;

                Console.WriteLine($"{new string('|', increment)}Finished w/{(success ? "Success" : "Failure")} {{{operation}}}");
            }
        }
    }
}
