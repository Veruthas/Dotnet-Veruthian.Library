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
using System.Collections.Generic;
using System.IO;

namespace _Console
{
    public static class TestParsers
    {
        private static void TestParser()
        {
            var data = GetBuilder(TypeSet.Create<ISpeculativeReader<Rune>, ISpeculative>());

            var b = data.Builder;

            var rule = data.Rules;

            var tracer = data.tracer;

            var Symbols = RuneSet.List("`~!@#$%^&*()-_=+[{]}\\|;:'\",<.>/?");

            // rule File = Whitespace Items unless Any;
            rule["File"] = b.Sequence(rule["Whitespace"], rule["Items"], b.Unless(b.MatchSet(RuneSet.Complete)));

            // Items = {Item};
            rule["Items"] = b.Repeat(rule["Item"]);

            // Item = Number or Word or Symbol;
            rule["Item"] = b.Choice(rule["Number"], rule["Word"], rule["Symbol"]);

            // rule Symbol = (at least 1 Symbol) Whitespace;
            rule["Symbol"] = b.Sequence(b.AtLeast(1, b.MatchSet(Symbols)), rule["Whitespace"]);

            // rule Number = (at least 1 Digit) Whitespace;
            rule["Number"] = b.Sequence(b.Classify(b.AtLeast(1, b.MatchSet(RuneSet.Digit)), "value"), rule["Whitespace"]);

            // rule Word = (at least 1 Letter) Whitespace;
            rule["Word"] = b.Sequence(b.Classify(b.AtLeast(1, b.MatchSet(RuneSet.Letter)), "value"), rule["Whitespace"]);

            // rule Whitespace = {Whitespace};
            rule["Whitespace"] = b.Repeat(b.MatchSet(RuneSet.Whitespace));



            // var items = rule["File"].Flatten();

            // foreach(var item in items)
            //     Console.WriteLine(item);

            Console.WriteLine(rule.FormatRule("File"));

            Console.WriteLine(rule.FormatRule("Items"));

            Console.WriteLine(rule.FormatRule("Item"));

            Console.WriteLine(rule.FormatRule("Symbol"));

            Console.WriteLine(rule.FormatRule("Number"));

            Console.WriteLine(rule.FormatRule("Word"));

            Console.WriteLine(rule.FormatRule("Whitespace").Replace("\t", "\\t"));


            Program.Pause();

            Console.WriteLine();

            while (true)
            {
                Console.Write("Parse String> ");

                // if (tracer is Capturer)
                //     tracer.Captures.Clear();

                var s = (RuneString)Console.ReadLine();

                for (int i = 0; i < 3; i++)
                {
                    tracer.Count = 0;

                    var r = s.GetSpeculativeReader();

                    bool result = rule.Perform("File", r.GroupWith((ISpeculative)r), tracer);

                    Console.WriteLine($"Result: {(result ? "Success" : "Failed")}");

                    Console.WriteLine(r.Position);

                    Console.WriteLine(r.IsEnd);

                    Console.WriteLine(tracer.Count);

                    // for (int i = 0; i < tracer.Captures.Count; i++)
                    // {
                    //     (int position, RuneString value) = tracer.Captures.Values[i];
                    //     Console.WriteLine($"[{i}] Position: {position}; Value: '{value}'");
                    // }

                    Program.Pause();

                    Console.WriteLine();

                    if (i == 0)
                    {
                        // rule File = Whitespace Items unless Any;
                        rule["File"] = b.Sequence(rule["Whitespace"], rule["Items"], b.Unless(b.MatchSet(RuneSet.Complete)));

                        // Items = {Item};
                        rule["Items"] = b.Repeat(rule["Item"]);

                        // Item = if Digit then Number else if Letter then Word else if Symbols then Symbol;
                        rule["Item"] = b.IfThenElse(b.MatchSet(RuneSet.Digit), rule["Number"], b.IfThenElse(b.MatchSet(RuneSet.Letter), rule["Word"], b.IfThen(b.MatchSet(Symbols), rule["Symbol"])));
                    }
                    else if (i == 1)
                    {
                        // rule File = Whitespace Items unless Any;
                        rule["File"] = b.Sequence(rule["Whitespace"], rule["Items"], b.Unless(b.MatchSet(RuneSet.Complete)));

                        // Items = while (Digit or Letter or Symbols) then Item;
                        rule["Items"] = b.While(b.MatchSet(RuneSet.Union(RuneSet.Digit, RuneSet.Letter, Symbols)), rule["Item"]);

                        // Item = if Digit then Number else if Letter then Word else if Symbols then Symbol;
                        rule["Item"] = b.IfThenElse(b.MatchSet(RuneSet.Digit), rule["Number"], b.IfThenElse(b.MatchSet(RuneSet.Letter), rule["Word"], b.IfThen(b.MatchSet(Symbols), rule["Symbol"])));
                    }
                    else
                    {
                        // rule File = Whitespace Items unless Any;
                        rule["File"] = b.Sequence(rule["Whitespace"], rule["Items"], b.Unless(b.MatchSet(RuneSet.Complete)));

                        // Items = {Item};
                        rule["Items"] = b.Repeat(rule["Item"]);

                        // Item = Number or Word or Symbol;
                        rule["Item"] = b.Choice(rule["Number"], rule["Word"], rule["Symbol"]);
                    }
                }
            }

        }


        static (AnalyzerBuilder<TState, ISpeculativeReader<Rune>, Rune> Builder, RuleTable<TState> Rules, Tracer<TState> tracer) GetBuilder<TState>(TState mockup)
            where TState : Has<ISpeculative>, Has<ISpeculativeReader<Rune>>
        {
            return (new AnalyzerBuilder<TState, ISpeculativeReader<Rune>, Rune>(), new RuleTable<TState>(), new Tracer<TState>());
        }

        public class Tracer<TState> : ITracer<TState>
        {
            public int Count;

            int increment = 0;

            public void OnStart(IOperation<TState> operation, TState state, out bool? handled)
            {
                Console.WriteLine($"{new string('|', increment)}Starting {{{operation}}}");

                handled = null;

                increment++;

                Count++;
            }

            public void OnFinish(IOperation<TState> operation, TState state, bool success)
            {
                increment--;

                Console.WriteLine($"{new string('|', increment)}Finished w/{(success ? "Success" : "Failure")} {{{operation}}}");
            }
        }

        public class Capturer<TState> : ITracer<TState>
            where TState : Has<ISpeculative>, Has<ISpeculativeReader<Rune>>
        {
            public SortedList<int, (int, RuneString)> Captures = new SortedList<int, (int, RuneString)>();

            public void OnStart(IOperation<TState> operation, TState state, out bool? handled)
            {
                var classified = operation as ClassifiedOperation<TState>;

                if (classified != null)
                {
                    if (classified.Classes.Contains("value"))
                    {
                        state.Get(out ISpeculative speculative);

                        speculative.Mark();
                    }
                }

                handled = null;
            }

            public void OnFinish(IOperation<TState> operation, TState state, bool success)
            {
                if (success)
                {
                    var classified = operation as ClassifiedOperation<TState>;

                    if (classified != null && classified.Classes.Contains("value"))
                    {
                        state.Get(out ISpeculativeReader<Rune> reader);

                        var runes = new RuneString(reader.PeekFromMark(0, null));

                        Captures.TryAdd(reader.MarkPosition, (reader.MarkPosition, runes));

                        reader.Commit();
                    }
                }
            }
        }
    }
}