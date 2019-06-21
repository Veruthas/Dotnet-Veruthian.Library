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
        public static IStep SpeculateIf(this StepGenerator generator, IStep condition, bool? setType = null)
            => generator.If(Speculate(condition), setType);

        public static IStep SpeculateIfThen(this StepGenerator generator, IStep condition, IStep thenStep, bool? setType = null)
            => generator.IfThen(Speculate(condition), thenStep, setType);

        public static IStep SpeculateIfElse(this StepGenerator generator, IStep condition, IStep elseStep, bool? setType = null)
            => generator.IfElse(Speculate(condition), elseStep, setType);

        public static IStep SpeculateIfThenElse(this StepGenerator generator, IStep condition, IStep thenStep, IStep elseStep, bool? setType = null)
            => generator.IfThenElse(Speculate(condition), thenStep, elseStep, setType);


        // Speculate Unless
        public static IStep SpeculateUnless(this StepGenerator generator, IStep condition, bool? setType = null)
            => generator.Unless(Speculate(condition), setType);

        public static IStep SpeculateUnlessThen(this StepGenerator generator, IStep condition, IStep thenStep, bool? setType = null)
            => generator.UnlessThen(Speculate(condition), thenStep, setType);

        public static IStep SpeculateUnlessElse(this StepGenerator generator, IStep condition, IStep elseStep, bool? setType = null)
            => generator.UnlessElse(Speculate(condition), elseStep, setType);

        public static IStep SpeculateUnlessThenElse(this StepGenerator generator, IStep condition, IStep thenStep, IStep elseStep, bool? setType = null)
            => generator.UnlessThenElse(Speculate(condition), thenStep, elseStep, setType);


        // Repeat
        public static IStep SpeculateRepeat(this StepGenerator generator, IStep step, bool? setType = null)
            => generator.While(Speculate(step), step, setType);
        
        public static IStep SpeculateAtMost(this StepGenerator generator, int times, IStep step, bool? setType = null)
            => generator.AtMost(times, Speculate(step), step, setType);

        public static IStep SpeculateAtLeast(this StepGenerator generator, int times, IStep step, bool? setType = null)
            => generator.AtLeast(times, Speculate(step), step, setType);

        public static IStep SpeculateBetween(this StepGenerator generator, int min, int max, IStep step, bool? setType = null)
            => generator.Between(min, max, Speculate(step), step, setType);
    }
}