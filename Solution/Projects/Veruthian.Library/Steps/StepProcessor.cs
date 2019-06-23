using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public delegate bool? StepTracer(IStep step, bool? state, bool completed);
    
    public delegate bool? StepTracer<T>(IStep step, bool? state, bool completed, T value);
    
    public delegate bool? StepTracer<T0, T1>(IStep step, bool? state, bool completed, T0 value0, T1 value1);

    public delegate bool? StepTracer<T0, T1, T2>(IStep step, bool? state, bool completed, T0 value0, T1 value1, T2 value2);


    public static class StepProcessor
    {
        
        public static void Process(IStep step, StepTracer tracer = null)
        {
            var walker = new StepWalker(step);

            while (!walker.WalkCompleted)
            {
                var current = walker.Step;

                switch (current)
                {
                    case IActionStep action:
                        walker.State = action.Act(walker.State, walker.StepCompleted);
                        break;
                }

                if (tracer != null)
                    walker.State = tracer(current, walker.State, walker.StepCompleted);

                walker.Walk();                
            }
        }

        public static void Process<T>(IStep step, T value, StepTracer<T> tracer = null)
        {
            var walker = new StepWalker(step);

            while (!walker.WalkCompleted)
            {
                var current = walker.Step;

                switch (current)
                {
                    case IActionStep action:
                        walker.State = action.Act(walker.State, walker.StepCompleted);
                        break;
                        
                    case IActionStep<T> action:
                        walker.State = action.Act(walker.State, walker.StepCompleted, value);
                        break;
                }

                if (tracer != null)
                    walker.State = tracer(current, walker.State, walker.StepCompleted, value);

                walker.Walk();
            }
        }

        public static void Process<T0, T1>(IStep step, T0 value0, T1 value1, StepTracer<T0, T1> tracer = null)
        {
            var walker = new StepWalker(step);

            while (!walker.WalkCompleted)
            {
                var current = walker.Step;

                switch (current)
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

                if (tracer != null)
                    walker.State = tracer(current, walker.State, walker.StepCompleted, value0, value1);


                walker.Walk();
            }
        }

        public static void Process<T0, T1, T2>(IStep step, T0 value0, T1 value1, T2 value2, StepTracer<T0, T1, T2> tracer = null)
        {
            var walker = new StepWalker(step);

            while (!walker.WalkCompleted)
            {
                var current = walker.Step;

                switch (current)
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

                if (tracer != null)
                    walker.State = tracer(current, walker.State, walker.StepCompleted, value0, value1, value2);

                walker.Walk();
            }
        }
    }
}