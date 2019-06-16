using System.Collections.Generic;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Steps
{
    public class StepGenerator
    {
        // Boolean
        public GeneralStep True
        {
            get
            {
                return new GeneralStep()
                {
                    Shunt = ShuntTrue,
                    Down = ShuntTrue
                };
            }
        }

        public GeneralStep False
        {
            get
            {
                return new GeneralStep()
                {
                    Shunt = ShuntTrue,
                    Down = ShuntFalse
                };
            }
        }


        private EmptyStep ShuntTrue => new EmptyStep();

        private EmptyStep ShuntFalse => null;


        // Typed
        public GeneralStep Typed(string type)
            => new GeneralStep(type);

        public GeneralStep Typed(string type, IStep step)
            => new GeneralStep(type) { Down = step };

        public GeneralStep Typed(string type, string name)
            => new GeneralStep(type, name);

        public GeneralStep Typed(string type, string name, IStep step)
            => new GeneralStep(type, name) { Down = step };


        // Sequence
        public GeneralStep Sequence(params IStep[] steps) => Sequence((IEnumerable<IStep>)steps);

        public GeneralStep Sequence(IEnumerable<IStep> steps)
        {
            var first = new GeneralStep();

            var current = first;

            foreach (var step in steps)
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


        // Choice
        public GeneralStep Choice(params IStep[] steps)
        {
            var first = new GeneralStep();

            var current = first;

            foreach (var step in steps)
            {
                if (current != first)
                {
                    var next = new GeneralStep();

                    current.Next = next;

                    current = next;
                }

                current.Shunt = current.Down = step;
            }

            return first;
        }


        // Optional
        public GeneralStep Optional(IStep step)
        {
            ExceptionHelper.VerifyNotNull(step, nameof(step));

            return new GeneralStep
            {
                Shunt = step,

                Down = step,

                Next = ShuntTrue
            };
        }


        // If
        public GeneralStep If(IStep condition)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            return new GeneralStep
            {
                Shunt = condition,

                Down = new EmptyStep(),

                Next = ShuntFalse
            };
        }

        public GeneralStep IfThen(IStep condition, IStep thenStep)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            ExceptionHelper.VerifyNotNull(thenStep, nameof(thenStep));

            return new GeneralStep
            {
                Shunt = condition,

                Down = thenStep,

                Next = ShuntFalse
            };
        }

        public GeneralStep IfElse(IStep condition, IStep elseStep)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            ExceptionHelper.VerifyNotNull(elseStep, nameof(elseStep));

            return new GeneralStep
            {
                Shunt = condition,

                Down = ShuntTrue,

                Next = elseStep
            };
        }

        public GeneralStep IfThenElse(IStep condition, IStep thenStep, IStep elseStep)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            ExceptionHelper.VerifyNotNull(thenStep, nameof(thenStep));

            ExceptionHelper.VerifyNotNull(elseStep, nameof(elseStep));

            return new GeneralStep
            {
                Shunt = condition,

                Down = thenStep,

                Next = elseStep
            };
        }


        // Unless
        public GeneralStep Unless(IStep condition)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            return new GeneralStep
            {
                Shunt = condition,

                Down = ShuntFalse,

                Next = ShuntTrue
            };
        }

        public GeneralStep UnlessThen(IStep condition, IStep thenStep)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            ExceptionHelper.VerifyNotNull(thenStep, nameof(thenStep));

            return new GeneralStep
            {
                Shunt = condition,

                Down = ShuntFalse,

                Next = thenStep
            };
        }

        public GeneralStep UnlessElse(IStep condition, IStep elseStep)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            ExceptionHelper.VerifyNotNull(elseStep, nameof(elseStep));

            return new GeneralStep
            {
                Shunt = condition,

                Down = elseStep,

                Next = ShuntTrue
            };
        }

        public GeneralStep UnlessThenElse(IStep condition, IStep thenStep, IStep elseStep)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            ExceptionHelper.VerifyNotNull(thenStep, nameof(thenStep));

            ExceptionHelper.VerifyNotNull(elseStep, nameof(elseStep));

            return new GeneralStep
            {
                Shunt = condition,

                Down = elseStep,

                Next = thenStep
            };
        }


        // Repeat
        public GeneralStep While(IStep condition, IStep step)
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

        public GeneralStep Until(IStep condition, IStep step)
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

        public GeneralStep Repeat(IStep step)
            => While(step, step);

        public GeneralStep Exactly(int times, IStep step)
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

        public GeneralStep AtMost(int times, IStep step)
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

                current.Shunt = step;

                var down = new GeneralStep();

                current.Down = down;

                down.Down = step;

                current = down;
            }

            return first;
        }

        public GeneralStep AtLeast(int times, IStep step)
        {
            var result = Exactly(times, step);

            result.Next = Repeat(step);

            return result;
        }

        public GeneralStep Between(int min, int max, IStep step)
        {
            var result = Exactly(min, step);

            result.Next = AtMost(max - min, step);

            return result;
        }
    }
}