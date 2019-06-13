using Veruthian.Library.Collections;

namespace Veruthian.Library.Patterns
{
    public class StepWalker
    {
        IStack<IStep> stack;

        IStep start;


        public StepWalker(IStep step)
        {
            this.start = step;

            this.stack = new DataStack<IStep>();

            stack.Push(step);
        }


        public IStep Step => stack.Count.IsZero ? null : stack.Peek();

        public bool StepCompleted { get; private set; }

        public void Walk(bool? state)
        {
            if (stack.Count.IsNotZero)
            {
                var step = stack.Peek();

                if (!StepCompleted)
                {
                    if (state == true)
                    {
                        if (step.Shunt != null)
                        {
                            stack.Push(step.Shunt);
                        }
                        else if (step.Down != null)
                        {
                            stack.Push(step.Down);
                        }
                        else if (step.Next != null)
                        {
                            stack.Replace(step.Next);
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
                        if (state == true && step.Down != null)
                        {
                            stack.Replace(step.Down);

                            StepCompleted = false;
                        }
                        else if (state == false && step.Next != null)
                        {
                            stack.Replace(step.Next);

                            StepCompleted = false;
                        }
                        else
                        {
                            stack.Pop();
                        }
                    }
                    else if (step.Next != null && state == true)
                    {
                        stack.Replace(step.Next);

                        StepCompleted = false;
                    }
                    else
                    {
                        stack.Pop();
                    }
                }
            }
        }

        public void Reset()
        {
            this.stack.Clear();

            this.stack.Push(start);

            StepCompleted = false;
        }
    }
}