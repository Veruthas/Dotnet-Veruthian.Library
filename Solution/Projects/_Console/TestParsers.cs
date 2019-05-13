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

            var state = GetState("Hello, world!");

            Console.WriteLine(rules);

            var result = handler.Handle(rules["File"], state);

            Console.WriteLine($"\nResult: {result.ToPrintable()}; Steps: {state.GetCounter()}");
        }



        private static StepTable GetRules()
        {
            var g = new MatchStepGenerator(new SpeculativeStepGenerator());

            var rules = new StepTable("rule");

            var tokens = rules.CreateSubTable("token");



            rules["File"] = g.Sequence(rules["Whitespace"], rules["Items"], g.Unless(g.MatchSet(RuneSet.Complete)));

            rules["Items"] = g.Repeat(rules["Item"]);

            rules["Item"] = g.Choice(rules["Symbol"], rules["Number"], rules["Word"]);

            tokens["Symbol"] = g.Sequence(g.AtLeast(1, g.MatchSet(RuneSet.Symbol)), rules["Whitespace"]);

            tokens["Number"] = g.Sequence(g.AtLeast(1, g.MatchSet(RuneSet.Digit)), rules["Whitespace"]);

            tokens["Word"] = g.Sequence(g.AtLeast(1, g.MatchSet(RuneSet.Letter)), rules["Whitespace"]);

            rules["Whitespace"] = g.Repeat(g.MatchSet(RuneSet.Whitespace));


            return rules;
        }

        static string ReaderAddress = "reader";

        static string SpeculativeAddress = "reader";

        static string CounterAddress = "counter";

        static string IndentAddress = "indent";

        public static IStepHandler<ILookup<string, object>> GetHandler()
        {
            var labeled = new LabeledStepHandler<ILookup<string, object>>();

            var handlers =
                new StepHandlerList<ILookup<string, object>>(
                    new LabeledStepHandler<ILookup<string, object>>(),
                    new BooleanStepHandler<ILookup<string, object>>(),
                    new SequentialStepHandler<ILookup<string, object>>(),
                    new OptionalStepHandler<ILookup<string, object>>(),
                    new RepeatedStepHandler<ILookup<string, object>>(),
                    new RepeatedTryStepHandler<ILookup<string, object>>(),
                    new SpeculativeConditonalStepHandler<ILookup<string, object>>(ReaderAddress),
                    new ReaderStepHandler<ILookup<string, object>, Rune>(SpeculativeAddress),
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

        public static ILookup<string, object> GetState(RuneString value)
        {
            var lookup = new DataLookup<string, object>();

            var reader = value.GetSpeculativeReader();

            lookup.Insert(ReaderAddress, reader);

            lookup.Insert(CounterAddress, new Ref<int>());

            lookup.Insert(IndentAddress, new Ref<int>());

            return lookup;
        }


        static string ToPrintable(this bool? value) => value == null ? "Unhandled" : value.ToString();


        static Ref<int> GetCounter(this ILookup<string, object> state)
        {
            state.Get(CounterAddress, out Ref<int> counter);

            return counter;
        }

        static Ref<int> GetIndent(this ILookup<string, object> state)
        {
            state.Get(IndentAddress, out Ref<int> indent);

            return indent;
        }

        static ISpeculativeReader<Rune> GetReader(this ILookup<string, object> state)
        {
            state.Get(ReaderAddress, out ISpeculativeReader<Rune> reader);

            return reader;
        }
    }
}