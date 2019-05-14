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
using Veruthian.Library.Types.Extensions;

namespace _Console
{
    public static class TestParsers
    {
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

        private static StepTable GetRules()
        {
            var g = new MatchStepGenerator(new SpeculativeStepGenerator());


            var rules = new StepTable(RuleLabel);

            var tokens = rules.CreateSubTable(TokenLabel);


            rules["File"] = g.Sequence(rules["Whitespace"], rules["Items"], g.Unless(g.MatchSet(RuneSet.Complete)));

            rules["Items"] = g.Repeat(rules["Item"]);

            rules["Item"] = g.Choice(rules["Symbol"], rules["Number"], rules["Word"]);

            tokens["Symbol"] = g.Sequence(g.AtLeast(1, g.MatchSet(RuneSet.Symbol)), rules["Whitespace"]);

            tokens["Number"] = g.Sequence(g.AtLeast(1, g.MatchSet(RuneSet.Digit)), rules["Whitespace"]);

            tokens["Word"] = g.Sequence(g.AtLeast(1, g.MatchSet(RuneSet.Letter)), rules["Whitespace"]);

            rules["Whitespace"] = g.Repeat(g.MatchSet(RuneSet.Whitespace));


            return rules;
        }

        static string RuleLabel = "rule";

        static string TokenLabel = "token";

        static string TokenFlagAddress = "tokenFlag";

        static string MemoizeFlagAddress = "memoizeFlag";

        static string HandledFlagAddress = "handledFlag";


        static string ReaderAddress = "reader";

        static string SpeculativeAddress = "reader";

        static string CounterAddress = "counter";

        static string IndentAddress = "indent";

        public static IStepHandler GetHandler()
        {
            var labeled = new LabeledStepHandler();
            
            labeled.StepStarted += (step, state) =>
            {
                if (step.Has(RuleLabel))
                {
                    state.SetFlag(HandledFlagAddress, true);

                    var reader = state.GetReader();

                    var progress = reader.RecallProgress(step);

                    if (progress.Success == true)
                    {
                        reader.Advance(progress.Length);

                        return true;
                    }
                    else if (progress.Success == false)
                    {
                        return false;
                    }
                }

                return null;
            };

            labeled.StepCompleted += (step, state, result) =>
            {
                if (step.Has(RuleLabel))
                {
                    if (result != null && !state.GetFlag(HandledFlagAddress) && state.GetFlag(MemoizeFlagAddress))
                    {
                        var reader = state.GetReader();

                        if (reader.IsSpeculating)
                            reader.StoreProgress(step, result.Value);
                    }

                    state.SetFlag(HandledFlagAddress, false);
                }
            };

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

            handlers.StepStarted += (step, state) =>
            {
                var counter = state.GetCounter();

                var indent = state.GetIndent();

                var reader = state.GetReader();

                counter.Value++;

                indent.Value++;


                Console.WriteLine($"{new string('|', indent)}Started @({reader.Position}:'{reader.Current.ToPrintableString()}') #{counter} {step}");

                return null;
            };

            handlers.StepCompleted += (step, state, result) =>
            {
                var indent = state.GetIndent();

                var reader = state.GetReader();

                Console.WriteLine($"{new string('|', indent)}Completed @({reader.Position}:'{reader.Current.ToPrintableString()}') <{result.ToPrintable()}> {step}");

                indent.Value--;
            };

            return handlers;
        }

        public static StateTable GetState(RuneString value)
        {
            var lookup = new StateTable();

            var reader = value.GetRecollectiveReader();

            lookup.Insert(ReaderAddress, reader);

            lookup.InsertRef(CounterAddress, 0);

            lookup.InsertRef(IndentAddress, 0);

            lookup.InsertRef(TokenFlagAddress, false);

            lookup.InsertRef(MemoizeFlagAddress, false);

            lookup.InsertRef(HandledFlagAddress, false);

            return lookup;
        }


        static string ToPrintable(this bool? value) => value == null ? "Unhandled" : value.ToString();


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
    }
}