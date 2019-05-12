namespace Veruthian.Library.Steps.Handlers
{
    public class BooleanStepHandler<TState> : StepHandler<TState, BooleanStep>
    {
        protected override bool? HandleStep(BooleanStep step, TState state, IStepHandler<TState> root)
        {
            return step.Value;
        }
    }
}