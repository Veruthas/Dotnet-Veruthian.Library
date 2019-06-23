namespace Veruthian.Library.Steps.Actions.Extensions
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
    }
}