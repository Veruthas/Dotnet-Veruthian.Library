using System;
using System.Runtime.CompilerServices;
using Veruthian.Library.Collections;
using Veruthian.Library.Numeric;
using Veruthian.Library.Processing;
using Veruthian.Library.Readers;
using Veruthian.Library.Readers.Extensions;
using Veruthian.Library.Steps;
using Veruthian.Library.Steps.Reader;
using Veruthian.Library.Steps.Speculate;
using Veruthian.Library.Text.Runes;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var rules = new StepTable<NestedStep>((name) => new NestedStep(name));

            var g = new NestedStepGenerator();

            // rule File = Space (Symbols / Number / Word) (unless Rune);
            rules["File"] = g.Sequence(rules["Space"], g.Choice(rules["Symbols"], rules["Number"], rules["Word"]), g.Unless(g.MatchSet(RuneSet.Complete)));

            // rule Symbols = (Symbol > 1) Space;
            rules["Symbols"] = g.Sequence(g.SpeculateAtLeast(1, g.MatchSet(RuneSet.Symbol)), rules["Space"]);

            // rule Number = (Digit > 1) Space;
            rules["Number"] = g.Sequence(g.SpeculateAtLeast(1, g.MatchSet(RuneSet.Digit)), rules["Space"]);

            // rule Word = (Letter > 1) Space;
            rules["Word"] = g.Sequence(g.SpeculateAtLeast(1, g.MatchSet(RuneSet.Letter)), rules["Space"]);

            // rule Space = Whitespace*;
            rules["Space"] = g.SpeculateRepeat(g.MatchSet(RuneSet.Whitespace));


            RuneString a = "Hello";

            var r = a.GetRecollectiveReader();

            reader = r;

            // r.Mark();

            // while (true)
            // {
            //     if (r.IsEnd)
            //     {
            //         Console.WriteLine($"{r.Position}:'{r.Current.ToPrintableString()}'");
                    
            //         if (r.IsSpeculating)
            //             r.Rollback();
            //         else
            //             break;
            //     }

            //     Console.WriteLine($"{r.Position}:'{r.Current.ToPrintableString()}'");

            //     r.Advance();
            // }


            var s = new Speculator(r);

            var table = new ObjectTable();

            MatchStep<Rune>.PrepareTable(table, r);

            SpeculateStep.PrepareTable(table, s);

            StepProcessor.Process(rules["Word"], table, Trace);


            Pause();
        }

        static Number id;

        static ConditionalWeakTable<IStep, string> names = new ConditionalWeakTable<IStep, string>();

        static int indent;

        static IReader<Rune> reader;

        public static bool? Trace(IStep step, bool? state, bool completed, ObjectTable table)
        {
            string name;

            if (!names.TryGetValue(step, out name))
            {
                name = id.ToHexadecimalString();

                id++;

                names.Add(step, name);
            }

            if (completed)
                indent--;

            Console.WriteLine($"{new string('|', indent)}{(step.ToString() ?? "Step")} {(completed ? "Completed" : "Started")}: {(state == null ? "Null" : state.ToString())} ({reader.Position}, '{reader.Current}')");

            if (!completed)
                indent++;

            return state;
        }

        public static void Pause()
        {
            Console.Write("Press any key to continue...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

            Console.WriteLine();
        }
    }
}
