namespace Veruthian.Library.Steps.Handlers
{
    public class RepeatedTryStepHandler<TState> : StepHandler<TState, RepeatedTryStep>
    {
        protected override bool? HandleStep(RepeatedTryStep step, TState state, IStepHandler<TState> root)
        {
            if (step.Count == null)
            {
                while (root.Handle(step.Step, state, root) == true)
                    ;
            }
            else
            {
                for (int i = 0; i < step.Count; i++)
                    if (root.Handle(step.Step, state, root) != true)
                        break;
            }

            return true;
        }
    }
}