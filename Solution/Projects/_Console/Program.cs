using System;
using System.Runtime.CompilerServices;
using Veruthian.Library.Numeric;
using Veruthian.Library.Steps;
using Veruthian.Library.Steps.Match;
using Veruthian.Library.Steps.Speculate;
using Veruthian.Library.Text.Runes;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = new NamedStepGenerator();

            var spaces = g.SpeculateRepeat(g.MatchSet(RuneSet.Whitespace));

            var word = g.Exactly(5, g.MatchSet(RuneSet.Letter));

            StepProcessor.Process(word, Trace);

            Pause();
        }

        static Number id;

        static ConditionalWeakTable<IStep, string> names = new ConditionalWeakTable<IStep, string>();

        static int indent;

        public static bool? Trace(IStep step, bool? state, bool completed)
        {
            string name;

            if (!names.TryGetValue(step, out name))
            {
                name = id.ToHexadecimalString(groupLength: 4, pad: true);

                id++;

                names.Add(step, name);
            }

            if (completed)
                indent--;

            Console.WriteLine($"{new string('|', indent)}{(step.ToString() ?? "Step")}:{name} {(completed ? "Completed" : "Started")}: {(state == null ? "null" : completed.ToString())}");

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
