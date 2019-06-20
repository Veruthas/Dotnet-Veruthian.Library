namespace Veruthian.Library.Steps.Actions.Extensions
{
    public static class ActionExtensions
    {
        // Speculate If
        public static IStep SpeculateIf(this StepGenerator generator, IStep condition)
            => generator.If(new SpeculateStep(condition));

        public static IStep SpeculateIfThen(this StepGenerator generator, IStep condition, IStep thenStep)
            => generator.IfThen(new SpeculateStep(condition), thenStep);

        public static IStep SpeculateIfElse(this StepGenerator generator, IStep condition, IStep elseStep)
            => generator.IfElse(new SpeculateStep(condition), elseStep);

        public static IStep SpeculateIfThenElse(this StepGenerator generator, IStep condition, IStep thenStep, IStep elseStep)
            => generator.IfThenElse(new SpeculateStep(condition), thenStep, elseStep);


        // Speculate Unless
        public static IStep SpeculateUnless(this StepGenerator generator, IStep condition)
            => generator.Unless(new SpeculateStep(condition));

        public static IStep SpeculateUnlessThen(this StepGenerator generator, IStep condition, IStep thenStep)
            => generator.UnlessThen(new SpeculateStep(condition), thenStep);

        public static IStep SpeculateUnlessElse(this StepGenerator generator, IStep condition, IStep elseStep)
            => generator.UnlessElse(new SpeculateStep(condition), elseStep);

        public static IStep SpeculateUnlessThenElse(this StepGenerator generator, IStep condition, IStep thenStep, IStep elseStep)
            => generator.UnlessThenElse(new SpeculateStep(condition), thenStep, elseStep);


        // Repeat
    }
}