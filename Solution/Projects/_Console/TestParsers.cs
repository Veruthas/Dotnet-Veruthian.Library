using System;
using System.Collections.Generic;
using Veruthian.Library.Collections;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;
using Veruthian.Library.Processing;
using Veruthian.Library.Readers;
using Veruthian.Library.Readers.Extensions;
using Veruthian.Library.Steps;
using Veruthian.Library.Steps.Generators;
using Veruthian.Library.Steps.Handlers;
using Veruthian.Library.Text.Runes;
using Veruthian.Library.Types;

namespace _Console
{
    public static class TestParsers
    {
        static string RuleLabel = "rule";

        static string TokenLabel = "token";

        static string LiteralLabel = "literal";

        static string TokenFlagAddress = "tokenFlag";

        static string MemoizeFlagAddress = "memoizeFlag";

        static string TraceFlagAddress = "traceFlag";


        static string ReaderAddress = "reader";

        static string SpeculativeAddress = "reader";

        static string CounterAddress = "counter";

        static string IndentAddress = "indent";


        public static void Test()
        {
            var rules = GetRules();

            var handler = GetHandler();

            Console.WriteLine(rules);


            RuneString data = "Hello, world!";

            ProcessData(rules, handler, data, false);

            Program.Pause();

            ProcessData(rules, handler, data, true);
        }

        private static void ProcessData(StepTable rules, IStepHandler handler, RuneString data, bool memoize)
        {
            var state = GetState(data);

            state.SetFlag(MemoizeFlagAddress, memoize);

            bool? result = handler.Handle(rules["File"], state);

            Console.WriteLine($"\nResult: {result.ToPrintable()}; Steps: {state.GetCounter()}");

            Console.WriteLine();
        }


        #region Rules

        private static StepTable GetRules()
        {
            var g = new MatchStepGenerator(new SpeculativeStepGenerator());

            var items = new StepTable(true);

            var rules = items.CreateSubTable(RuleLabel);

            var tokens = rules.CreateSubTable(TokenLabel);

            // rule File = Whitespace Items unless Any;
            rules["File"] = g.Sequence(rules["Whitespace"], rules["Items"], g.Unless(g.MatchSet(RuneSet.Complete)));

            // rule Items = {Item};
            rules["Items"] = g.Repeat(rules["Item"]);

            // rule Item = Symbol or Number or Word
            rules["Item"] = g.Choice(rules["Symbols"], rules["Number"], rules["Word"]);

            // rule:token Symbols = (at least 1 Symbol):Literal Whitespace;
            tokens["Symbols"] = g.Sequence(g.Literal(g.AtLeast(1, g.MatchSet(RuneSet.Symbol))), rules["Whitespace"]);

            // rule:token Number = (at least 1 Digit):Literal Whitespace
            tokens["Number"] = g.Sequence(g.Literal(g.AtLeast(1, g.MatchSet(RuneSet.Digit))), rules["Whitespace"]);

            // rule:token Word = (at least 1 Letter):Literal Whitespace
            tokens["Word"] = g.Sequence(g.Literal(g.AtLeast(1, g.MatchSet(RuneSet.Letter))), rules["Whitespace"]);

            // rule Whitespace = {SpaceOrNewline};
            rules["Whitespace"] = g.Repeat(g.MatchSet(RuneSet.Whitespace));


            return rules;
        }

        static LabeledStep Literal(this IStepGenerator generator, IStep step) => generator.Label(step, LiteralLabel);

        #endregion

        #region Handler

        public static IStepHandler GetHandler()
        {
            var labeled = new LabeledStepHandler();

            var handlers =
                new StepHandlerList(
                    new BooleanStepHandler(),
                    new SequentialStepHandler(),
                    new OptionalStepHandler(),
                    new RepeatedStepHandler(),
                    new RepeatedTryStepHandler(),
                    new SpeculativeConditonalStepHandler(ReaderAddress),
                    new ReaderStepHandler<Rune>(SpeculativeAddress),
                    labeled
                );

            labeled.StepStarted += LabeledStepStarted;

            labeled.StepCompleted += LabeledStepCompleted;

            handlers.StepStarted += StepStarted;

            handlers.StepCompleted += StepCompleted;

            return handlers;
        }


        // Step Events
        private static bool? StepStarted(IStep step, StateTable state)
        {
            var counter = state.GetCounter();

            var indent = state.GetIndent();

            var reader = state.GetReader();

            counter.Value++;

            indent.Value++;

            if (state.GetFlag(TraceFlagAddress))
                Console.WriteLine($"{new string('|', indent)}Started @({reader.Position}:'{reader.Current.ToPrintableString()}') #{counter} {step}");

            return null;
        }

        private static void StepCompleted(IStep step, StateTable state, bool? result, bool handled)
        {
            var h = handled;

            var indent = state.GetIndent();

            var reader = state.GetReader();

            if (state.GetFlag(TraceFlagAddress))
                Console.WriteLine($"{new string('|', indent)}Completed @({reader.Position}:'{reader.Current.ToPrintableString()}') <{result.ToPrintable()}> {step}");

            indent.Value--;
        }



        // Labeled Step Events
        private static bool? LabeledStepStarted(LabeledStep step, StateTable state)
        {
            // Rule
            if (step.Has(RuleLabel))
            {
                var result = CheckProgress(step, state, true);

                if (result.Result != null)
                    return result.Result;
            }

            // Token
            if (step.Has(TokenLabel))
            {
                state.SetFlag(TokenFlagAddress, true);
            }

            // Literal
            if (step.Has(LiteralLabel) && state.GetFlag(TokenFlagAddress))
            {
                var result = CheckProgress(step, state, false);

                if (result.Result != null)
                {
                    if (result.Result == true)
                        Console.WriteLine($"*******RECALLED FOUND Literal: '{result.Data ?? ""}'");
                    else
                        Console.WriteLine($"*******RECALLED MISSING Literal!");

                    return result.Result;
                }
                else
                {
                    var reader = state.GetReader();

                    reader.Mark();

                    Console.WriteLine($"*******SEEKING Literal");
                }
            }

            return null;
        }

        private static void LabeledStepCompleted(LabeledStep step, StateTable state, bool? result, bool handled)
        {
            if (handled)
                return;

            // Rule
            if (step.Has(RuleLabel))
            {
                if (result != null && !handled && state.GetFlag(MemoizeFlagAddress))
                {
                    var reader = state.GetReader();

                    if (reader.IsSpeculating)
                        reader.StoreProgress(step, result.Value);
                }
            }

            // Token
            if (step.Has(TokenLabel))
            {
                state.SetFlag(TokenFlagAddress, false);
            }

            // Literal
            if (step.Has(LiteralLabel) && state.GetFlag(TokenFlagAddress))
            {
                var reader = state.GetReader();

                if (result == true)
                {
                    var literal = RuneString.Withdraw(reader.LookFromMark(0, null));

                    if (reader.IsSpeculating && state.GetFlag(MemoizeFlagAddress))
                        reader.StoreProgress(step, true, literal);

                    Console.WriteLine($"*******FOUND Literal: '{literal}'");
                }
                else
                {
                    if (reader.IsSpeculating && state.GetFlag(MemoizeFlagAddress))
                        reader.StoreProgress(step, false);

                    Console.WriteLine($"*******MISSING Literal!");
                }

                reader.Commit();
            }
        }


        private static (bool? Result, object Data) CheckProgress(IStep step, StateTable state, bool whenSpeculating)
        {
            var reader = state.GetReader();

            if (!whenSpeculating || reader.IsSpeculating)
            {
                var progress = reader.RecallProgress(step);

                if (progress.Success == true)
                {
                    reader.Advance(progress.Length);

                    return (progress.Success, progress.Data);
                }
                else if (progress.Success == false)
                {
                    return (progress.Success, progress.Data);
                }
            }

            return (null, null);
        }


        static string ToPrintable(this bool? value) => value == null ? "Unhandled" : value.ToString();

        #endregion

        #region State

        public static StateTable GetState(RuneString value)
        {
            var lookup = new StateTable();

            var reader = value.GetRecollectiveReader();

            lookup.Insert(ReaderAddress, reader);

            lookup.InsertRef(CounterAddress, 0);

            lookup.InsertRef(IndentAddress, 0);

            lookup.InsertRef(TokenFlagAddress, false);

            lookup.InsertRef(MemoizeFlagAddress, false);

            lookup.InsertRef(TraceFlagAddress, true);

            return lookup;
        }


        // State Property
        static Ref<int> GetCounter(this StateTable state)
        {
            state.Get(CounterAddress, out Ref<int> counter);

            return counter;
        }

        static Ref<int> GetIndent(this StateTable state)
        {
            state.Get(IndentAddress, out Ref<int> indent);

            return indent;
        }

        static IRecollectiveReader<Rune> GetReader(this StateTable state)
        {
            state.Get(ReaderAddress, out IRecollectiveReader<Rune> reader);

            return reader;
        }

        #endregion
    }
}