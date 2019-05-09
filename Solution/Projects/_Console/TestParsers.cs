// using System;
// using Veruthian.Library.Numeric;
// using Veruthian.Library.Operations;
// using Veruthian.Library.Operations.Extensions;
// using Veruthian.Library.Operations.Analyzers;
// using Veruthian.Library.Processing;
// using Veruthian.Library.Readers;
// using Veruthian.Library.Readers.Extensions;
// using Veruthian.Library.Text.Chars.Extensions;
// using Veruthian.Library.Text.Runes;
// using Veruthian.Library.Types;
// using Veruthian.Library.Types.Extensions;
// using System.Collections.Generic;
// using System.IO;
// using Veruthian.Library.Operations.Analyzers.Extensions;

// namespace _Console
// {
//     public static class TestParsers
//     {
//         public static void TestParser()
//         {
//             var data = GetBuilder(TypeSet.Create<IRecollectiveReader<Rune>, ISpeculative>());

//             var b = data.Builder;

//             var rule = data.Rules;

//             var tracer = data.tracer;

//             var Symbols = RuneSet.List("`~!@#$%^&*()-_=+[{]}\\|;:'\",<.>/?");


//             // rule Symbol = (at least 1 Symbol) Whitespace;
//             rule["Symbol", AnalyzerClass.Token] = b.Sequence(b.Literal(b.AtLeast(1, b.MatchSet(Symbols))), rule["Whitespace"]);

//             // rule Number = (at least 1 Digit) Whitespace;
//             rule["Number", AnalyzerClass.Token] = b.Sequence(b.Literal(b.AtLeast(1, b.MatchSet(RuneSet.Digit))), rule["Whitespace"]);

//             // rule Word = (at least 1 Letter) Whitespace;
//             rule["Word", AnalyzerClass.Token] = b.Sequence(b.Literal(b.AtLeast(1, b.MatchSet(RuneSet.Letter))), rule["Whitespace"]);

//             // rule Whitespace = {Whitespace};
//             rule["Whitespace"] = b.Repeat(b.MatchSet(RuneSet.Whitespace));

//             Console.WriteLine(rule);

//             Console.WriteLine();

//             while (true)
//             {
//                 Program.Pause();

//                 Console.WriteLine();

//                 Console.Write("Parse String> ");

//                 // if (tracer is Capturer)
//                 //     tracer.Captures.Clear();

//                 var s = ((RuneString)Console.ReadLine() * 100);


//                 for (int i = 0; i < 6; i++)
//                 {
//                     var optimizeLevel = i / 2;

//                     if (optimizeLevel == 0)
//                     {
//                         // rule File = Whitespace Items unless Any;
//                         rule["File"] = b.Sequence(rule["Whitespace"], rule["Items"], b.Unless(b.MatchSet(RuneSet.Complete)));

//                         // Items = {Item};
//                         rule["Items"] = b.Repeat(rule["Item"]);

//                         // Item = Number or Word or Symbol;
//                         rule["Item"] = b.Choice(rule["Number"], rule["Word"], rule["Symbol"]);
//                     }
//                     else if (optimizeLevel == 1)
//                     {
//                         // rule File = Whitespace Items unless Any;
//                         rule["File"] = b.Sequence(rule["Whitespace"], rule["Items"], b.Unless(b.MatchSet(RuneSet.Complete)));

//                         // Items = {Item};
//                         rule["Items"] = b.Repeat(rule["Item"]);

//                         // Item = if Digit then Number else if Letter then Word else if Symbols then Symbol;
//                         rule["Item"] = b.IfThenElse(b.MatchSet(RuneSet.Digit), rule["Number"], b.IfThenElse(b.MatchSet(RuneSet.Letter), rule["Word"], b.IfThen(b.MatchSet(Symbols), rule["Symbol"])));
//                     }
//                     else
//                     {
//                         // rule File = Whitespace Items unless Any;
//                         rule["File"] = b.Sequence(rule["Whitespace"], rule["Items"], b.Unless(b.MatchSet(RuneSet.Complete)));

//                         // Items = while (Digit or Letter or Symbols) then Item;
//                         rule["Items"] = b.While(b.MatchSet(RuneSet.Union(RuneSet.Digit, RuneSet.Letter, Symbols)), rule["Item"]);

//                         // Item = if Digit then Number else if Letter then Word else if Symbols then Symbol;
//                         rule["Item"] = b.IfThenElse(b.MatchSet(RuneSet.Digit), rule["Number"], b.IfThenElse(b.MatchSet(RuneSet.Letter), rule["Word"], b.IfThen(b.MatchSet(Symbols), rule["Symbol"])));
//                     }

//                     var watch = new System.Diagnostics.Stopwatch();

//                     watch.Start();

//                     Console.WriteLine();

//                     tracer.Count = 0;

//                     tracer.Captures.Clear();

//                     var r = s.GetRecollectiveReader<Rune>();

//                     bool result = rule.Perform("File", r.GroupWith((ISpeculative)r), tracer);

//                     watch.Stop();

//                     Console.WriteLine((tracer.StashDiscoveries ? "Stashed" : "UnStashed") + "; Level: " + optimizeLevel);

//                     Console.WriteLine($"Result: {(result ? "Success" : "Failed")}");

//                     Console.WriteLine("Position: " + r.Position);

//                     Console.WriteLine("IsEnd: " + r.IsEnd);

//                     Console.WriteLine("Tracer Count: " + tracer.Count);

//                     Console.WriteLine("Token Count: " + tracer.Captures.Count);

//                     Console.WriteLine($"Elapsed millisceconds: {watch.ElapsedMilliseconds}");


//                     tracer.StashDiscoveries ^= true;

//                     //tracer.OutputTrace ^= true;

//                     // for (int j = 0; j < tracer.Captures.Count; j++)
//                     // {
//                     //     (int position, RuneString value) = tracer.Captures.Values[j];
//                     //     Console.WriteLine($"[{j}] Position: {position}; Value: '{value.ToPrintableString()}'");
//                     // }
//                 }
//             }
//         }


//         static (MatchOperationGenerator<TState, IRecollectiveReader<Rune>, Rune> Builder, RuleTable<TState> Rules, Tracer<TState> tracer) GetBuilder<TState>(TState mockup)
//             where TState : Has<ISpeculative>, Has<IRecollectiveReader<Rune>>
//         {
//             return (new MatchOperationGenerator<TState, IRecollectiveReader<Rune>, Rune>(new SpeculativeOperationGenerator<TState>()),
//                     new RuleTable<TState>(),
//                     new Tracer<TState>());
//         }

//         public class Tracer<TState> : ITracer<TState>
//             where TState : Has<ISpeculative>, Has<IRecollectiveReader<Rune>>
//         {
//             public SortedList<int, (int, RuneString)> Captures = new SortedList<int, (int, RuneString)>();

//             public int Count;

//             public bool OutputTrace;

//             public bool StashDiscoveries;


//             int increment;

//             bool token = false;


//             public bool? OperationStart(IOperation<TState> operation, TState state)
//             {
//                 Count++;

//                 state.Get(out IRecollectiveReader<Rune> reader);

//                 PrintStart(operation, reader.Position, reader.Peek());

//                 var classified = operation as ClassifiedOperation<TState>;

//                 if (classified != null)
//                 {
//                     if (classified.IsRule())
//                     {
//                         if (StashDiscoveries)
//                         {
//                             var result = reader.RecallProgress(operation);

//                             if (result.Success == true)
//                             {
//                                 reader.Skip(result.Length);

//                                 PrintFinish(operation, true, true, reader.Position, reader.Peek());

//                                 return true;
//                             }
//                             else if (result.Success == false)
//                             {
//                                 PrintFinish(operation, false, true, reader.Position, reader.Peek());

//                                 return false;
//                             }
//                         }
//                     }

//                     if (classified.IsToken())
//                     {
//                         token = true;
//                     }

//                     if (token && classified.IsLiteral())
//                     {
//                         state.Get(out ISpeculative speculative);

//                         speculative.Mark();
//                     }
//                 }

//                 increment++;

//                 return null;
//             }

//             public void OperationFinish(IOperation<TState> operation, TState state, bool success)
//             {
//                 increment--;

//                 state.Get(out IRecollectiveReader<Rune> reader);

//                 PrintFinish(operation, success, false, reader.Position, reader.Peek());

//                 var classified = operation as ClassifiedOperation<TState>;

//                 if (classified != null)
//                 {
//                     if (classified.IsRule())
//                     {
//                         if (StashDiscoveries)
//                         {

//                             if (reader.IsSpeculating)
//                             {
//                                 reader.StoreProgress(operation, success);
//                             }
//                         }
//                     }

//                     if (classified.Is("Token"))
//                     {
//                         token = false;
//                     }

//                     if (token && classified.IsLiteral())
//                     {
//                         if (success)
//                         {
//                             var runes = new RuneString(reader.PeekFromMark(0, null));

//                             Captures.TryAdd(reader.MarkPosition, (reader.MarkPosition, runes));
//                         }

//                         reader.Commit();
//                     }
//                 }
//             }

//             private void PrintStart(IOperation<TState> operation, int position, Rune value)
//             {
//                 if (OutputTrace)
//                     Console.WriteLine($"{new string('|', increment)}Starting @({position},'{value.ToPrintableString()}') {{{operation.ToString().ToPrintableString()}}}");
//             }

//             private void PrintFinish(IOperation<TState> operation, bool success, bool skipped, int position, Rune value)
//             {
//                 if (OutputTrace)
//                     Console.WriteLine($"{new string('|', increment)}Finished @({position},'{value.ToPrintableString()}') w/{(skipped ? "Skip-" : "")}{(success ? "Success" : "Failure")} {{{operation.ToString().ToPrintableString()}}}");
//             }


//             public void Reset()
//             {
//                 Captures.Clear();

//                 Count = 0;

//                 increment = 0;

//                 token = false;
//             }
//         }
//     }
// }

using System;
using Veruthian.Library.Numeric;
using Veruthian.Library.Steps;
using Veruthian.Library.Steps.Matching;
using Veruthian.Library.Text.Runes;

namespace _Console
{
    public static class TestParsers
    {
        public static void Test()
        {
            var g = new MatchStepGenerator(new SpeculativeStepGenerator());

            var rules = new StepTable("rule");

            var tokens = rules.CreateSubTable("token");

            // rules
            rules["Files"] = g.Sequence(rules["Whitespace"], rules["Items"], g.Unless(g.MatchSet(RuneSet.Complete)));

            rules["Items"] = g.Repeat(rules["Item"]);

            rules["Item"] = g.Choice(rules["Symbol"], rules["Number"], rules["Word"]);

            tokens["Symbol"] = g.Sequence(g.AtLeast(1, g.MatchSet(RuneSet.Symbol)), rules["Whitespace"]);

            tokens["Number"] = g.Sequence(g.AtLeast(1, g.MatchSet(RuneSet.Digit)), rules["Whitespace"]);

            tokens["Word"] = g.Sequence(g.AtLeast(1, g.MatchSet(RuneSet.Letter)), rules["Whitespace"]);

            rules["Whitespace"] = g.Repeat(g.MatchSet(RuneSet.Whitespace));


            Console.WriteLine(rules);
        }
    }
}