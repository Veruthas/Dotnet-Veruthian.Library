using System.Collections.Generic;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Steps
{
    public class StepGenerator
    {
        private Number linkNumber = Number.Zero;

        public bool NameLinks { get; set; }

        public bool TypeConstructs { get; set; }


        protected string NewName() => NameLinks ? NewName() : null;

        protected GeneralStep NewLinkStep(IStep shunt = null, IStep down = null, IStep next = null)
            => new GeneralStep(name: NewName()) { Shunt = shunt, Down = down, Next = next };

        protected IStep ResultStep(IStep step, string type = null)
            => (TypeConstructs && type != null) ? Typed(type, NewName(), step) : step;


        // Typed
        public IStep Typed(string type)
            => new GeneralStep(type);

        public IStep Typed(string type, IStep step)
            => new GeneralStep(type) { Down = step };

        public IStep Typed(string type, string name)
            => new GeneralStep(type, name);

        public IStep Typed(string type, string name, IStep step)
            => new GeneralStep(type, name) { Down = step };


        // Sequence
        public const string SequenceType = "Sequence";

        public IStep Sequence(params IStep[] steps)
            => Sequence(steps as IEnumerable<IStep>);

        public IStep Sequence(IEnumerable<IStep> steps)
            => ResultStep(RawSequence(steps), SequenceType);

        protected IStep RawSequence(IEnumerable<IStep> steps)
        {
            var first = NewLinkStep();

            var current = first;

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
            return first;
        }



        // Condition
        protected IStep Condition(IStep condition, IStep onTrue, IStep onFalse, string type = null)
        {
            var result = NewLinkStep(shunt: condition, down: onTrue, next: onFalse);

            return ResultStep(result, type);
        }

        protected IStep ShuntTrue => new TerminalStep(name: NewName());

        protected IStep ShuntFalse => null;


        // Boolean
        public const string TrueType = "True";

        public IStep True => ResultStep(RawTrue, TrueType);

        protected IStep RawTrue => NewLinkStep(shunt: ShuntTrue, down: ShuntTrue);


        public const string FalseType = "False";

        public IStep False => ResultStep(RawFalse, FalseType);

        protected IStep RawFalse => NewLinkStep(shunt: ShuntTrue, down: ShuntFalse);




        // If
        public const string IfType = "If";

        public const string IfThenType = "IfThen";

        public const string IfElseType = "IfElse";

        public const string IfThenElseType = "IfThenElse";


        public IStep If(IStep condition)
            => Condition(condition, onTrue: ShuntTrue, onFalse: ShuntFalse, type: IfType);

        public IStep IfThen(IStep condition, IStep thenStep)
            => Condition(condition, onTrue: thenStep, onFalse: ShuntFalse, type: IfThenType);

        public IStep IfElse(IStep condition, IStep elseStep)
            => Condition(condition, onTrue: ShuntTrue, onFalse: elseStep, type: IfElseType);

        public IStep IfThenElse(IStep condition, IStep thenStep, IStep elseStep)
            => Condition(condition, onTrue: thenStep, onFalse: elseStep, type: IfThenElseType);


        // Unless
        public const string UnlessType = "Unless";

        public const string UnlessThenType = "UnlessThen";

        public const string UnlessElseType = "UnlessElse";

        public const string UnlessThenElseType = "UnlessThenElse";


        public IStep Unless(IStep condition)
            => Condition(condition, onTrue: ShuntFalse, onFalse: ShuntTrue, type: UnlessType);

        public IStep UnlessThen(IStep condition, IStep thenStep)
            => Condition(condition, onTrue: ShuntFalse, onFalse: thenStep, type: UnlessThenType);

        public IStep UnlessElse(IStep condition, IStep elseStep)
            => Condition(condition, onTrue: elseStep, onFalse: ShuntTrue, type: UnlessElseType);

        public IStep UnlessThenElse(IStep condition, IStep thenStep, IStep elseStep)
            => Condition(condition, onTrue: elseStep, onFalse: thenStep, type: UnlessThenElseType);


        // Repeat
        public const string WhileType = "While";

        public const string UntilType = "Until";

        public const string ExactlyType = "Exactly";

        public const string AtMostType = "AtMost";

        public const string AtLeastType = "AtLeast";

        public const string BetweenType = "Between";


        public IStep While(IStep condition, IStep step)
            => ResultStep(RawWhile(condition, step), WhileType);


        public IStep Until(IStep condition, IStep step)
            => ResultStep(RawUntil(condition, step), UntilType);

        public IStep Exactly(int times, IStep step)
            => ResultStep(RawExactly(times, step), ExactlyType);

        public IStep AtMost(int times, IStep condition, IStep step)
            => ResultStep(RawAtMost(times, condition, step), AtMostType);

        public IStep AtLeast(int times, IStep condition, IStep step)
            => ResultStep(RawAtLeast(times, condition, step), AtLeastType);

        public IStep Between(int min, int max, IStep condition, IStep step)
            => ResultStep(RawBetween(min, max, condition, step), BetweenType);


        protected GeneralStep RawWhile(IStep condition, IStep step)
        {
            var repeater = NewLinkStep();

            repeater.Shunt = condition;

            repeater.Down = NewLinkStep
            (
                down: step,

                next: repeater
            );

            repeater.Next = ShuntTrue;

            return repeater;
        }

        protected GeneralStep RawUntil(IStep condition, IStep step)
        {
            var repeater = NewLinkStep();

            repeater.Shunt = condition;

            repeater.Down = ShuntTrue;

            repeater.Next = NewLinkStep
            (
                down: step,

                next: repeater
            );

            return repeater;
        }

        protected GeneralStep RawExactly(int times, IStep step)
        {
            var first = NewLinkStep();

            var current = first;

            for (int i = 0; i < times; i++)
            {
                if (current != first)
                {
                    var next = NewLinkStep();

                    current.Next = next;

                    current = next;
                }

                current.Down = step;
            }

            return first;
        }

        protected GeneralStep RawAtMost(int times, IStep condition, IStep step)
        {
            var first = NewLinkStep();

            var current = first;

            for (int i = 0; i < times; i++)
            {
                if (current != first)
                {
                    var next = NewLinkStep();

                    current.Next = next;

                    current = next;
                }

                current.Shunt = condition;

                var down = NewLinkStep();

                current.Down = down;

                down.Down = step;

                current = down;
            }

            return first;
        }

        protected GeneralStep RawAtLeast(int times, IStep condition, IStep step)
        {
            var result = RawExactly(times, step);

            result.Next = RawWhile(condition, step);

            return result;
        }

        protected GeneralStep RawBetween(int min, int max, IStep condition, IStep step)
        {
            var result = RawExactly(min, step);

            result.Next = RawAtMost(max - min, condition, step);

            return result;
        }
    }
}