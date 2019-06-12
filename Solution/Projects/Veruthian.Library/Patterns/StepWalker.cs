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

        public IStep Current => steps.Count.IsZero ? null : steps.Peek();

        public bool CurrentCompleted { get; private set; }

        public void Walk(bool status)
        {
            if (steps.Count.IsNotZero)
            {
                var step = steps.Peek();

                if (!CurrentCompleted)
                {
                    if (status)
                    {
                        if (step.Shunt != null)
                        {
                            steps.Push(step);
                        }
                        else if (step.Down != null)
                        {
                            steps.Push(step.Down);
                        }
                        else
                        {
                            CurrentCompleted = true;
                        }
                    }
                    else
                    {
                        CurrentCompleted = true;
                    }
                }
                else
                {
                    if (step.Shunt != null)
                    {
                        if (status & step.Down != null)
                        {
                            steps.Replace(step.Down);

                            CurrentCompleted = false;
                        }
                        else if (!status & step.Next != null)
                        {
                            steps.Replace(step.Next);

                            CurrentCompleted = false;
                        }
                        else
                        {
                            steps.Pop();
                        }

                        CurrentCompleted = false;
                    }
                    else if (step.Next != null)
                    {
                        steps.Replace(step.Next);

                        CurrentCompleted = false;
                    }
                    else
                    {
                        steps.Pop();
                    }
                }
            }
        }
    }
}