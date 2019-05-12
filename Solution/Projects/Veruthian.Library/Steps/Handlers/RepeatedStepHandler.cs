namespace Veruthian.Library.Steps.Handlers
{
    public class RepeatedStepHandler<TState> : StepHandler<TState, RepeatedStep>
    {
        protected override bool? HandleStep(RepeatedStep step, TState state, IStepHandler<TState> root)
        {
            for (int i = 0; i < step.Count; i++)
            {
                var result = root.Handle(step.Step, state, root);

                if (result != true)
                    return false;
            }

            return true;
        }
    }

}