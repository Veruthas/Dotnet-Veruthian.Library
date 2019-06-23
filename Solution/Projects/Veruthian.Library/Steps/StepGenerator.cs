using System.Collections.Generic;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Steps
{
    public class StepGenerator
    {
        // Sequence
        public virtual IStep Sequence(params IStep[] steps)
            => Sequence(steps as IEnumerable<IStep>);

        public virtual IStep Sequence(IEnumerable<IStep> steps)
        {
            var first = new LinkStep();

            var current = null as LinkStep;

            foreach (var step in steps)
            {
                if (current == null)
                {
                    current = first;
                }
                else
                {
                    var next = new LinkStep();

                    current.Next = next;

                    current = next;
                }

                current.Down = step;
            }
            return first;
        }


        // Conditions
        private IStep Condition(IStep condition, IStep onTrue, IStep onFalse)
            => new LinkStep { Shunt = condition, Down = onTrue, Next = onFalse };

        private IStep ShuntTrue => new EmptyStep();

        private IStep ShuntFalse => null;



        // Boolean
        public virtual IStep True => new LinkStep { Shunt = ShuntTrue, Down = ShuntTrue };

        public virtual IStep False => new LinkStep { Shunt = ShuntTrue, Down = ShuntFalse };



        // If
        public virtual IStep If(IStep condition)
            => Condition(condition, ShuntTrue, ShuntFalse);

        public virtual IStep IfThen(IStep condition, IStep thenStep)
            => Condition(condition, thenStep, ShuntFalse);

        public virtual IStep IfElse(IStep condition, IStep elseStep)
            => Condition(condition, ShuntTrue, elseStep);

        public virtual IStep IfThenElse(IStep condition, IStep thenStep, IStep elseStep)
            => Condition(condition, thenStep, elseStep);


        // Unless
        public virtual IStep Unless(IStep condition)
            => Condition(condition, ShuntFalse, ShuntTrue);

        public virtual IStep UnlessThen(IStep condition, IStep thenStep)
            => Condition(condition, ShuntFalse, thenStep);

        public virtual IStep UnlessElse(IStep condition, IStep elseStep)
            => Condition(condition, elseStep, ShuntTrue);

        public virtual IStep UnlessThenElse(IStep condition, IStep thenStep, IStep elseStep)
            => Condition(condition, elseStep, thenStep);


        // Repeat
        public virtual IStep While(IStep condition, IStep step)
        {
            var repeater = new LinkStep();

            repeater.Shunt = condition;

            repeater.Down = new LinkStep
            {
                Down = step,

                Next = repeater
            };

            repeater.Next = ShuntTrue;

            return repeater;
        }

        public virtual IStep Until(IStep condition, IStep step)
        {
            var repeater = new LinkStep();

            repeater.Shunt = condition;

            repeater.Down = ShuntTrue;

            repeater.Next = new LinkStep
            {
                Down = step,

                Next = repeater
            };

            return repeater;
        }

        public virtual IStep Exactly(int times, IStep step)
            => RawExactly(times, step);

        private LinkStep RawExactly(int times, IStep step)
        {
            var first = new LinkStep();

            var current = first;

            for (int i = 0; i < times; i++)
            {
                if (i > 0)
                {
                    var next = new LinkStep();

                    current.Next = next;

                    current = next;
                }

                current.Down = step;
            }

            return first;
        }

        public virtual IStep AtMost(int times, IStep condition, IStep step)
        {
            var first = new LinkStep();

            var current = first;

            for (int i = 0; i < times; i++)
            {
                if (i > 0)
                {
                    var next = new LinkStep();

                    current.Next = next;

                    current = next;
                }

                current.Shunt = condition;

                var down = new LinkStep();

                current.Down = down;

                down.Down = step;

                current = down;
            }

            return first;
        }

        public virtual IStep AtLeast(int times, IStep condition, IStep step)
        {
            var result = RawExactly(times, step);

            result.Next = While(condition, step);

            return result;
        }

        public virtual IStep Between(int min, int max, IStep condition, IStep step)
        {
            var result = RawExactly(min, step);

            result.Next = AtMost(max - min, condition, step);

            return result;
        }
    }
}