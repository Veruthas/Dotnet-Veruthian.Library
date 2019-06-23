using System.Collections.Generic;

namespace Veruthian.Library.Steps.Speculate
{
    public static class SpeculateExtensions
    {
        // Speculate
        private static IStep Speculate(IStep step)
            => new SpeculateStep(step);

        public static IStep Speculate(this StepGenerator generator, IStep step)
            => Speculate(step);

        // Speculate If
        public static IStep SpeculateIf(this StepGenerator generator, IStep condition)
            => generator.If(Speculate(condition));

        public static IStep SpeculateIfThen(this StepGenerator generator, IStep condition, IStep thenStep)
            => generator.IfThen(Speculate(condition), thenStep);

        public static IStep SpeculateIfElse(this StepGenerator generator, IStep condition, IStep elseStep)
            => generator.IfElse(Speculate(condition), elseStep);

        public static IStep SpeculateIfThenElse(this StepGenerator generator, IStep condition, IStep thenStep, IStep elseStep)
            => generator.IfThenElse(Speculate(condition), thenStep, elseStep);


        // Speculate Unless
        public static IStep SpeculateUnless(this StepGenerator generator, IStep condition)
            => generator.Unless(Speculate(condition));

        public static IStep SpeculateUnlessThen(this StepGenerator generator, IStep condition, IStep thenStep)
            => generator.UnlessThen(Speculate(condition), thenStep);

        public static IStep SpeculateUnlessElse(this StepGenerator generator, IStep condition, IStep elseStep)
            => generator.UnlessElse(Speculate(condition), elseStep);

        public static IStep SpeculateUnlessThenElse(this StepGenerator generator, IStep condition, IStep thenStep, IStep elseStep)
            => generator.UnlessThenElse(Speculate(condition), thenStep, elseStep);


        // Repeat
        public static IStep SpeculateRepeat(this StepGenerator generator, IStep step)
            => generator.While(Speculate(step), step);

        public static IStep SpeculateAtMost(this StepGenerator generator, int times, IStep step)
            => generator.AtMost(times, Speculate(step), step);

        public static IStep SpeculateAtLeast(this StepGenerator generator, int times, IStep step)
            => generator.AtLeast(times, Speculate(step), step);

        public static IStep SpeculateBetween(this StepGenerator generator, int min, int max, IStep step)
            => generator.Between(min, max, Speculate(step), step);


        // Choice
        private static IStep Choice(IEnumerable<IStep> steps)
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

                current.Shunt = Speculate(step);

                current.Down = step;
            }

            return first;
        }

        public static IStep Choice(this StepGenerator generator, IEnumerable<IStep> steps)
            => Choice(steps);

        public static IStep Choice(this StepGenerator generator, params IStep[] steps)
            => Choice(steps);

        public static IStep Choice(this NestedStepGenerator generator, IEnumerable<IStep> steps)
            => new NestedStep("Choice", Choice(steps));

        public static IStep Choice(this NestedStepGenerator generator, params IStep[] steps)
            => new NestedStep("Choice", Choice(steps));
    }
}