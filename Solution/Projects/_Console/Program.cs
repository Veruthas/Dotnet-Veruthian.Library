using System;
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

            g.NameAllSteps = true;

            g.TypeAllConstructs = true;

            var letter = new MatchSetStep<Rune>(RuneSet.Letter);

            var word = g.Exactly(4, letter);

            StepProcessor.Process(word, (step, state, completed) => { Console.WriteLine($"{step} {(completed ? "Completed" : "Started")}: {(state == null ? "null" : state.ToString())}"); return state; });

            Pause();
        }

        public static void Pause()
        {
            Console.Write("Press any key to continue...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

            Console.WriteLine();
        }
    }
}
