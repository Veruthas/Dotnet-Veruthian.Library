namespace Veruthian.Library.Steps.Generators
{
    public class SpeculativeStepGenerator : StepGenerator
    {
        // Optional
        public override OptionalStep Optional(IStep step)
            => base.Optional(IfThen(step, step));


        // Repeat
        public override RepeatedTryStep Repeat(IStep step)
            => new RepeatedTryStep(IfThen(step, step));

        public override RepeatedTryStep AtMost(int times, IStep step)
            => new RepeatedTryStep(IfThen(step, step), times);


        // Choice
        protected override IStep ChoiceFinal(IStep final)
            => IfThen(final, final);

        protected override IStep ChoiceComponent(IStep previous, IStep next)
            => IfThenElse(previous, previous, next);
    }
}