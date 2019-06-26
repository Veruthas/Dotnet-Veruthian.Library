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

            if (start != null)
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

        public bool WalkCompleted => stack.Count.IsZero;

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
                        else
                        {
                            completed = true;
                        }
                    }
                    else if (state == null)
                    {
                        completed = true;

                        state = true;
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
                        if (state == null)
                        {
                            stack.Pop();

                            state = true;
                        }
                        else
                        {
                            var next = state == true ? step.Down : step.Next;

                            if (next != null)
                            {
                                stack.Replace(next);

                                completed = false;

                                state = true;
                            }
                            else
                            {
                                stack.Pop();

                                state = false;
                            }
                        }
                    }
                    else if (step.Next != null && state == true)
                    {
                        stack.Replace(step.Next);

                        completed = false;
                    }
                    else if (state == null)
                    {
                        stack.Pop();

                        state = true;
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