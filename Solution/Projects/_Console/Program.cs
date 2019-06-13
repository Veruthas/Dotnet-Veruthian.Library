using System;
using Veruthian.Library.Patterns;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {

            var g = new StepGenerator();

            var a = g.Sequence(
                new NamedStep("A"),
                new NamedStep("B"),
                new NamedStep("C")
                {
                    Shunt = new NamedStep("Condition")
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
                },
                new NamedStep("D")
                {
                    Down = new NamedStep("E")
                    {
                        Down = new NamedStep("F")
                        {
                            Down = new NamedStep("G")
                        }
                    }
                }
            );


            var w = new StepWalker(a);

            while (w.Step != null)
            {
                switch (w.Step)
                {
                    case BooleanStep truth:
                        Console.WriteLine($"{truth.Value} {(w.StepCompleted ? "Completed" : "Started")}  {w.State}");
                        if (w.StepCompleted)
                            w.State = truth.Value;
                        break;
                    case NamedStep named:
                        Console.WriteLine($"{named} {(w.StepCompleted ? "Completed" : "Started")} {w.State}");
                        if (named.Name == "Condition" && !w.StepCompleted)
                            w.State = false;
                        break;
                    default:
                        Console.WriteLine($"{w.Step} {(w.StepCompleted ? "Completed" : "Started")} {w.State}");
                        if (!w.StepCompleted)
                            w.State = true;
                        break;
                }

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
