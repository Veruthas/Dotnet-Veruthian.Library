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
                new Step("A"),
                new Step("B"),
                new Step("C")
                {
                    Shunt = new Step("Condition")
                    {
                        Down = BooleanStep.True
                    },
                    Down = new Step
                    {
                        Next = new Step("Then")
                        {
                            Down = new Step("ThenDown"),
                            Next = new Step("ThenNext")
                        }
                    },
                    Next = new Step
                    {
                        Next = new Step("Else")
                        {
                            Down = new Step("ElseDown"),
                            Next = new Step("ElseNext")
                        }
                    }
                },
                new Step("D")
                {
                    Down = new Step("E")
                    {
                        Down = new Step("F")
                        {
                            Down = new Step("G")
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
                    case Step named:
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
