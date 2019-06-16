using System;
using Veruthian.Library.Steps;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new GeneralStep("A")
            {
                Shunt = new GeneralStep("B")
                {
                    Shunt = new GeneralStep("C"),
                    Down = null
                },
                Next = new EmptyStep("E"),
                Down = new EmptyStep("F")
            };

            var w = new StepWalker(a);

            while (w.Step != null)
            {
                Console.WriteLine($"Step {w.Step} {(w.StepCompleted ? "Completed" : "Started")} @ {(w.State == null ? "null" : w.State.Value.ToString())}");

                w.Walk();
            }

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
