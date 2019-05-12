namespace Veruthian.Library.Steps.Handlers
{
    public class OptionalStepHandler<TState> : StepHandler<TState, OptionalStep>
    {
        protected override bool? HandleStep(OptionalStep step, TState state, IStepHandler<TState> root)
        {
            root.Handle(step.Step, state, root);

            return true;
        }
    }
}