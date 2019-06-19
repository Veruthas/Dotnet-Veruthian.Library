using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public static class StepProcessor
    {
        public static void Process(IStep step)
        {
            var walker = new StepWalker(step);

            while (!walker.WalkCompleted)
            {
                var current = walker.Step;

                switch (step)
                {
                    case IActionStep action:
                        walker.State = action.Act(walker.State, walker.StepCompleted);
                        break;
                }

                walker.Walk();
            }
        }

        public static void Process<T>(IStep step, T value)
        {
            var walker = new StepWalker(step);

            while (!walker.WalkCompleted)
            {
                var current = walker.Step;

                switch (step)
                {
                    case IActionStep action:
                        walker.State = action.Act(walker.State, walker.StepCompleted);
                        break;
                        
                    case IActionStep<T> action:
                        walker.State = action.Act(walker.State, walker.StepCompleted, value);
                        break;
                }

                walker.Walk();
            }
        }

        public static void Process<T0, T1>(IStep step, T0 value0, T1 value1)
        {
            var walker = new StepWalker(step);

            while (!walker.WalkCompleted)
            {
                var current = walker.Step;

                switch (step)
                {
                    case IActionStep action:
                        walker.State = action.Act(walker.State, walker.StepCompleted);
                        break;
                    
                    case IActionStep<T0> action:
                        walker.State = action.Act(walker.State, walker.StepCompleted, value0);
                        break;

                    case IActionStep<T1> action:
                        walker.State = action.Act(walker.State, walker.StepCompleted, value1);
                        break;
                }

                walker.Walk();
            }
        }

        public static void Process<T0, T1, T2>(IStep step, T0 value0, T1 value1, T2 value2)
        {
            var walker = new StepWalker(step);

            while (!walker.WalkCompleted)
            {
                var current = walker.Step;

                switch (step)
                {
                    case IActionStep action:
                        walker.State = action.Act(walker.State, walker.StepCompleted);
                        break;

                    case IActionStep<T0> action:
                        walker.State = action.Act(walker.State, walker.StepCompleted, value0);
                        break;

                    case IActionStep<T1> action:
                        walker.State = action.Act(walker.State, walker.StepCompleted, value1);
                        break;

                    case IActionStep<T2> action:
                        walker.State = action.Act(walker.State, walker.StepCompleted, value2);
                        break;
                }

                walker.Walk();
            }
        }
    }
}