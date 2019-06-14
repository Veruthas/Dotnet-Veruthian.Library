using System;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public class StepWalker
    {
        IStack<IStep> stack;

        IStep start;

        bool? state;

        bool completed;


        public StepWalker(IStep step)
        {
            this.start = step;

            this.stack = new DataStack<IStep>();

            Reset();
        }

        public void Reset()
        {
            this.stack.Clear();

            this.stack.Push(start);

            state = true;

            completed = false;
        }


        public IStep Step => stack.Count.IsZero ? null : stack.Peek();

        public bool? State
        {
            get => state;
            set => this.state = value;
        }

        public bool StepCompleted => completed;



        public void Walk()
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
                            completed = true;
                        }
                    }
                    else
                    {
                        completed = true;
                    }
                }
                else
                {
                    if (step.Shunt != null)
                    {
                        if (state == true && step.Down != null)
                        {
                            stack.Replace(step.Down);

                            completed = false;
                        }
                        else if (state == false && step.Next != null)
                        {
                            stack.Replace(step.Next);

                            completed = false;
                        }
                        else
                        {
                            stack.Pop();
                        }

                        state = true;
                    }
                    else if (step.Next != null && state == true)
                    {
                        stack.Replace(step.Next);

                        completed = false;
                    }
                    else
                    {
                        stack.Pop();
                    }
                }
            }
        }


    }
}