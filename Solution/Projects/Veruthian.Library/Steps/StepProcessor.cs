using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public class StepProcessor
    {
        public void Process(IStep step, ObjectTable states)
        {
            var walker = new StepWalker(step);

            while (!walker.WalkCompleted)
            {
                var current = walker.Step;

                var action = current as IActionStep;

                if (action != null)
                    action.Act(walker, states);

                walker.Walk();
            }
        }
    }
}