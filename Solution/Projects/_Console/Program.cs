using System;
using Veruthian.Library.Numeric;
using Veruthian.Library.Operations;
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


            // rule File = Whitespace {Number or Word} unless Any;
            rule["File"] = b.Sequence(rule["Whitespace"], b.Repeat(b.Choice(rule["Number"], rule["Word"])), b.Unless(b.MatchSet(RuneSet.Complete)));

            // rule Number = (at least 1 <'0' to '9'>) Whitespace;
            rule["Number"] = b.Sequence(b.AtLeast(1, b.MatchSet(RuneSet.Digit)), rule["Whitespace"]);

            // rule Word = (at least 1 <'A' to 'Z' + 'a' to 'z'>) Whitespace;
            rule["Word"] = b.Sequence(b.AtLeast(1, b.MatchSet(RuneSet.Letter)), rule["Whitespace"]);

            // rule Whitespace = {<' ' + '\t'>};
            rule["Whitespace"] = b.Repeat(b.MatchSet(RuneSet.SpaceOrTab));


            //Console.WriteLine(rule["File"]);

            while (true)
            {
                Console.Write("Parse String> ");
                var s = (RuneString)Console.ReadLine();

                var r = s.GetSpeculativeReader();

                bool result = rule["File"].Perform(r.GroupWith((ISpeculative)r));

                Console.WriteLine($"Result: {(result ? "Success" : "Failed")}");

                Console.WriteLine();
            }

        }


        static (AnalyzerBuilder<TState, ISpeculativeReader<Rune>, Rune> Builder, RuleTable<TState> Rules) GetBuilder<TState>(TState mockup)
            where TState : Has<ISpeculative>, Has<ISpeculativeReader<Rune>>
        {
            return (new AnalyzerBuilder<TState, ISpeculativeReader<Rune>, Rune>(), new RuleTable<TState>());
        }
    }
}
