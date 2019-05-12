namespace Veruthian.Library.Steps.Handlers
{
    public class SequentialStepHandler<TState> : StepHandler<TState, SequentialStep>
    {
        protected override bool? HandleStep(SequentialStep step, TState state, IStepHandler<TState> root)
        {
            foreach (var substep in step.Sequence)
            {
                var result = root.Handle(substep, state, root);

                if (result != true)
                    return result;
            }

            return true;
        }
    }
}