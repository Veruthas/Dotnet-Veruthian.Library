using Veruthian.Library.Utility;

namespace Veruthian.Library.Patterns
{
    public class StepGenerator
    {
        public Step True => BooleanStep.True;

        public Step False => BooleanStep.False;

        public Step Null => BooleanStep.Null;


        // Sequence
        public Step Sequence(params IStep[] steps)
        {
            var first = new GeneralStep("Sequence");

            var current = first;

            foreach (var step in steps)
            {
                if (current == first)
                {
                    current = new GeneralStep();

                    first.Down = current;
                }
                else
                {
                    var next = new GeneralStep();

                    current.Next = next;

                    current = next;
                }

                current.Down = step;
            }

            return first;
        }


        // Optional
        public Step Optional(IStep step)
        {
            ExceptionHelper.VerifyNotNull(step, nameof(step));

            return new GeneralStep("If")
            {
                Shunt = step,
                Down = step,
                Next = True
            };
        }


        // If
        public Step If(IStep condition)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            return new GeneralStep("If")
            {
                Shunt = condition,
                Down = True,
                Next = False
            };
        }

        public Step IfThen(IStep condition, IStep thenStep)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            ExceptionHelper.VerifyNotNull(thenStep, nameof(thenStep));

            return new GeneralStep("IfThen")
            {
                Shunt = condition,
                Down = thenStep,
                Next = False
            };
        }

        public Step IfElse(IStep condition, IStep elseStep)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            ExceptionHelper.VerifyNotNull(elseStep, nameof(elseStep));

            return new GeneralStep("IfElse")
            {
                Shunt = condition,
                Down = True,
                Next = elseStep
            };
        }

        public Step IfThenElse(IStep condition, IStep thenStep, IStep elseStep)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            ExceptionHelper.VerifyNotNull(thenStep, nameof(thenStep));

            ExceptionHelper.VerifyNotNull(elseStep, nameof(elseStep));

            return new GeneralStep("IfThenElse")
            {
                Shunt = condition,
                Down = thenStep,
                Next = elseStep
            };
        }


        // Unless
        public Step Unless(IStep condition)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            return new GeneralStep("Unless")
            {
                Shunt = condition,
                Down = False,
                Next = True
            };
        }

        public Step UnlessThen(IStep condition, IStep thenStep)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            ExceptionHelper.VerifyNotNull(thenStep, nameof(thenStep));

            return new GeneralStep("UnlessThen")
            {
                Shunt = condition,
                Down = False,
                Next = thenStep
            };
        }

        public Step UnlessElse(IStep condition, IStep elseStep)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            ExceptionHelper.VerifyNotNull(elseStep, nameof(elseStep));

            return new GeneralStep("UnlessElse")
            {
                Shunt = condition,
                Down = elseStep,
                Next = True
            };
        }

        public Step UnlessThenElse(IStep condition, IStep thenStep, IStep elseStep)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            ExceptionHelper.VerifyNotNull(thenStep, nameof(thenStep));

            ExceptionHelper.VerifyNotNull(elseStep, nameof(elseStep));

            return new GeneralStep("UnlessThenElse")
            {
                Shunt = condition,
                Down = elseStep,
                Next = thenStep
            };
        }


        // Repeat
        public Step Repeat(IStep step)
        {
            var repeater = new GeneralStep("Repeat");

            repeater.Shunt = step;
            
            repeater.Next = True;

            repeater.Down = new GeneralStep
            {
                Down = step,
                Next = repeater
            };

            return repeater;
        }
    }
}