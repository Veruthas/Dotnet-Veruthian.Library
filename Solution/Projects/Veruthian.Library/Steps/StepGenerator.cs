using System.Collections.Generic;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Steps
{
    public class StepGenerator
    {
        private Number linkNumber = Number.Zero;

        public bool NameLinks { get; set; }

        public bool TypeConstructs { get; set; }


        protected GeneralStep NewLinkStep()
            => NameLinks ? new GeneralStep(name: $"Step #{linkNumber++}") : new GeneralStep();

        protected IStep ResultStep(string type, string name, IStep step)
            => TypeConstructs && type != null ? Typed(type, name, step) : step;

        protected IStep ShuntTrue => new EmptyStep();

        protected IStep ShuntFalse => null;


        // Typed
        public IStep Typed(string type)
            => new GeneralStep(type);

        public IStep Typed(string type, IStep step)
            => new GeneralStep(type) { Down = step };

        public IStep Typed(string type, string name)
            => new GeneralStep(type, name);

        public IStep Typed(string type, string name, IStep step)
            => new GeneralStep(type, name) { Down = step };



        // Boolean
        public IStep True
        {
            get
            {
                var step = NewLinkStep();

                step.Shunt = ShuntTrue;

                step.Down = ShuntTrue;

                return ResultStep("Boolean", "True", step);
            }
        }

        public IStep False
        {
            get
            {
                var step = NewLinkStep();
                
                step.Shunt = ShuntTrue;

                step.Down = ShuntFalse;

                return ResultStep("Boolean", "False", step);
            }
        }



        // Sequence
        public IStep Sequence(params IStep[] steps)
            => Sequence(steps as IEnumerable<IStep>);

        public IStep Sequence(IEnumerable<IStep> steps)
        {
            GeneralStep first = NewLinkStep();

            GeneralStep current = first;

            foreach (var step in steps)
            {
                if (current != first)
                {
                    var next = NewLinkStep();

                    current.Next = next;

                    current = next;
                }

                current.Down = step;
            }

            return ResultStep("Sequence", null, first);
        }


        // If
        public IStep If(IStep condition)
        {
            return new GeneralStep
            {
                Shunt = condition,

                Down = ShuntTrue,

                Next = ShuntFalse
            };
        }

        public IStep IfThen(IStep condition, IStep thenStep)
        {
            return new GeneralStep
            {
                Shunt = condition,

                Down = thenStep,

                Next = ShuntFalse
            };
        }

        public IStep IfElse(IStep condition, IStep elseStep)
        {
            return new GeneralStep
            {
                Shunt = condition,

                Down = ShuntTrue,

                Next = elseStep
            };
        }

        public IStep IfThenElse(IStep condition, IStep thenStep, IStep elseStep)
        {
            return new GeneralStep
            {
                Shunt = condition,

                Down = thenStep,

                Next = elseStep
            };
        }


        // Unless
        public IStep Unless(IStep condition)
        {
            return new GeneralStep
            {
                Shunt = condition,

                Down = ShuntFalse,

                Next = ShuntTrue
            };
        }

        public IStep UnlessThen(IStep condition, IStep thenStep)
        {
            return new GeneralStep
            {
                Shunt = condition,

                Down = ShuntFalse,

                Next = thenStep
            };
        }

        public IStep UnlessElse(IStep condition, IStep elseStep)
        {
            return new GeneralStep
            {
                Shunt = condition,

                Down = elseStep,

                Next = ShuntTrue
            };
        }

        public IStep UnlessThenElse(IStep condition, IStep thenStep, IStep elseStep)
        {
            return new GeneralStep
            {
                Shunt = condition,

                Down = elseStep,

                Next = thenStep
            };
        }


        // Repeat
        public IStep While(IStep condition, IStep step)
        {
            var repeater = new GeneralStep();

            repeater.Shunt = condition;

            repeater.Down = new GeneralStep
            {
                Down = step,

                Next = repeater
            };

            repeater.Next = ShuntTrue;

            return repeater;
        }

        public IStep Until(IStep condition, IStep step)
        {
            var repeater = new GeneralStep();

            repeater.Shunt = condition;

            repeater.Down = ShuntTrue;

            repeater.Next = new GeneralStep
            {
                Down = step,

                Next = repeater
            };

            return repeater;
        }

        public IStep Exactly(int times, IStep step)
            => Exactly(times, step, true);

        public IStep AtMost(int times, IStep condition, IStep step)
        {
            var first = new GeneralStep();

            var current = first;

            for (int i = 0; i < times; i++)
            {
                if (current != first)
                {
                    var next = new GeneralStep();

                    current.Next = next;

                    current = next;
                }

                current.Shunt = condition;

                var down = new GeneralStep();

                current.Down = down;

                down.Down = step;

                current = down;
            }

            return first;
        }

        public IStep AtLeast(int times, IStep condition, IStep step)
        {
            var result = Exactly(times, step, false);

            result.Next = While(condition, step);

            return result;
        }

        public IStep Between(int min, int max, IStep condition, IStep step)
        {
            var result = Exactly(min, step, false);

            result.Next = AtMost(max - min, condition, step);

            return result;
        }

        private GeneralStep Exactly(int times, IStep step, bool label)
        {
            var first = new GeneralStep();

            var current = first;

            for (int i = 0; i < times; i++)
            {
                if (current != first)
                {
                    var next = new GeneralStep();

                    current.Next = next;

                    current = next;
                }

                current.Down = step;
            }

            return first;
        }
    }
}