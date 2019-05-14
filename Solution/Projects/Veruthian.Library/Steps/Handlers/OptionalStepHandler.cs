namespace Veruthian.Library.Steps.Handlers
{
    public class OptionalStepHandler : StepHandler<OptionalStep>
    {
        protected override bool? HandleStep(OptionalStep step, StateTable state, IStepHandler root)
        {
            root.Handle(step.Step, state, root);

            return true;
        }
    }
}