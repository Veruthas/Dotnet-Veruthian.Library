using Veruthian.Library.Collections;

namespace Veruthian.Library.Patterns
{
    public class StepWalker
    {
        IStack<IStep> steps;

        public StepWalker(IStep step)
        {
            this.steps = new DataStack<IStep>();

            steps.Push(step);
        }

        public void Walk(bool status)
        {
            if (steps.Count != 0)
            {
                var step = steps.Peek();

                if (status)
                {                              
                }
                else
                {                    
                }
            }
        }
    }
}