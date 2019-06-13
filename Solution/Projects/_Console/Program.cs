using System;
using Veruthian.Library.Patterns;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {

            var g = new StepGenerator();

            var a = g.Sequence(new NamedStep("A"), new NamedStep("B"), new NamedStep("C")
            {
                Shunt = new Step
                {
                    Down = BooleanStep.True
                },
                Down = new Step
                {
                    Next = new NamedStep("Then")
                    {
                        Down = new NamedStep("ThenDown"),
                        Next = new NamedStep("ThenNext")
                    }
                },
                Next = new Step
                {
                    Next = new NamedStep("Else")
                    {
                        Down = new NamedStep("ElseDown"),
                        Next = new NamedStep("ElseNext")
                    }
                }
            });


            var w = new StepWalker(a);

            var state = true;

            while (w.Step != null)
            {
                switch (w.Step)
                {
                    case BooleanStep truth:
                        Console.WriteLine($"{truth.Value} {(w.StepCompleted ? "Completed" : "Started")}");
                        if (w.StepCompleted)
                            state = truth.Value;
                        break;
                    case NamedStep named:
                        Console.WriteLine($"{named} {(w.StepCompleted ? "Completed" : "Started")}");
                        break;
                    default:
                        //Console.WriteLine($"{w.Step} {(w.StepCompleted ? "Completed" : "Started")}");
                        if (!w.StepCompleted)
                            state = true;
                        break;
                }

                w.Walk(state);
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
