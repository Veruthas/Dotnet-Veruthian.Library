namespace Veruthian.Library.Steps.Handlers
{
    public class BooleanStepHandler : StepHandler<BooleanStep>
    {
        protected override bool? HandleStep(BooleanStep step, StateTable state, IStepHandler root)
        {
            return step.Value;
        }
    }
}