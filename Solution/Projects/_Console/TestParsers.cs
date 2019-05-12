using System;
using System.Collections.Generic;
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


            //var handler = GetHandler(TypeSets.Create<IReader<Rune>, ISpeculative>());


            Console.WriteLine(rules);
        }

        private static StepTable GetRules()
        {
            var g = new MatchStepGenerator(new SpeculativeStepGenerator());

            var rules = new StepTable("rule");

            var tokens = rules.CreateSubTable("token");



            rules["Files"] = g.Sequence(rules["Whitespace"], rules["Items"], g.Unless(g.MatchSet(RuneSet.Complete)));

            rules["Items"] = g.Repeat(rules["Item"]);

            rules["Item"] = g.Choice(rules["Symbol"], rules["Number"], rules["Word"]);

            tokens["Symbol"] = g.Sequence(g.AtLeast(1, g.MatchSet(RuneSet.Symbol)), rules["Whitespace"]);

            tokens["Number"] = g.Sequence(g.AtLeast(1, g.MatchSet(RuneSet.Digit)), rules["Whitespace"]);

            tokens["Word"] = g.Sequence(g.AtLeast(1, g.MatchSet(RuneSet.Letter)), rules["Whitespace"]);

            rules["Whitespace"] = g.Repeat(g.MatchSet(RuneSet.Whitespace));


            return rules;
        }

        public static IStepHandler<TState> GetHandler<T, TState>(TState state)
            where TState : Has<IReader<T>>, Has<ISpeculative>
        {
            return new StepHandlerList<TState>(
                new BooleanStepHandler<TState>(),
                new SequentialStepHandler<TState>(),
                new OptionalStepHandler<TState>(),
                new RepeatedStepHandler<TState>(),
                new RepeatedTryStepHandler<TState>(),
                new SpeculativeConditonalStepHandler<TState>(),
                new ReaderStepHandler<TState, T>()
            );
        }
    }
}