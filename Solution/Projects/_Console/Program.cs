using System;
using System.Runtime.CompilerServices;
using Veruthian.Library.Numeric;
using Veruthian.Library.Steps;
using Veruthian.Library.Steps.Actions;
using Veruthian.Library.Text.Runes;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = new StepGenerator();

            g.TypeAllConstructs = true;

            var letter = new MatchSetStep<Rune>(RuneSet.Letter);

            var word = g.Exactly(4, letter);

            StepProcessor.Process(word, Trace);

            Pause();
        }

        static Number id;

        static ConditionalWeakTable<IStep, string> names = new ConditionalWeakTable<IStep, string>();

        public static bool? Trace(IStep step, bool? state, bool completed)
        {
            string name;

            if (!names.TryGetValue(step, out name))
            {
                name = id.ToHexadecimalString(groupLength: 8, pad: true);

                id++;

                names.Add(step, name);
            }

            Console.WriteLine($"{step}:{name} {(completed ? "Completed" : "Started")}: {(state == null ? "null" : completed.ToString())}");

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
