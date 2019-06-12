using Veruthian.Library.Collections;

namespace Veruthian.Library.Patterns
{
    public class StepWalker
    {
        IStack<IStep> steps;

        IStep start;
        

        public StepWalker(IStep step)
        {
            this.start = step;

            this.steps = new DataStack<IStep>();

            steps.Push(step);
        }


        public IStep Step => steps.Count.IsZero ? null : steps.Peek();

        public bool StepCompleted { get; private set; }

        public void Walk(bool status)
        {
            if (steps.Count.IsNotZero)
            {
                var step = steps.Peek();

                if (!StepCompleted)
                {
                    if (status)
                    {
                        if (step.Shunt != null)
                        {
                            steps.Push(step.Shunt);
                        }
                        else if (step.Down != null)
                        {
                            steps.Push(step.Down);
                        }
                        else
                        {
                            StepCompleted = true;
                        }
                    }
                    else
                    {
                        StepCompleted = true;
                    }
                }
                else
                {
                    if (step.Shunt != null)
                    {
                        if (status & step.Down != null)
                        {
                            steps.Replace(step.Down);

                            StepCompleted = false;
                        }
                        else if (!status & step.Next != null)
                        {
                            steps.Replace(step.Next);

                            StepCompleted = false;
                        }
                        else
                        {
                            steps.Pop();
                        }

                        StepCompleted = false;
                    }
                    else if (step.Next != null)
                    {
                        steps.Replace(step.Next);

                        StepCompleted = false;
                    }
                    else
                    {
                        steps.Pop();
                    }
                }
            }
        }
    
        public void Reset()
        {
            this.steps.Clear();

            this.steps.Push(start);

            StepCompleted = false;
        }
    }
}