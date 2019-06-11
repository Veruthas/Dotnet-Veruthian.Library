using Veruthian.Library.Collections;

namespace Veruthian.Library.Patterns
{
    public class StepWalker
    {
        public void Walk(IStack<IStep> steps, bool status)
        {
            if (steps.Count != 0)
            {
                if (status)
                {
                    var step = steps.Peek();

                    if (step.Shunt != null)
                    {
                        steps.Push(step.Shunt);
                    }
                    else if (step.Left != null)
                    {
                        if (step.Right != null)
                            steps.Push(step.Left);
                        else
                            steps.Replace(step.Left);
                    }
                    else if (step.Right != null)
                    {
                        steps.Replace(step.Right);
                    }
                    else
                    {
                        steps.Pop();

                        if (steps.Count > 0)
                        {
                            step = steps.Peek();

                            if (step.Shunt != null)
                                steps.Replace(step.Left);
                            else
                                steps.Replace(step.Right);
                        }
                    }
                }
                else
                {
                    while (steps.Count != 0)
                    {
                        var step = steps.Peek();

                        if (step.Shunt != null)
                        {
                            steps.Replace(step.Right);
                            break;
                        }
                        else
                            steps.Pop();
                    }
                }
            }
        }
    }
}