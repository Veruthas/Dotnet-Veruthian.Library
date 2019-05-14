namespace Veruthian.Library.Steps.Handlers
{
    public class SequentialStepHandler : StepHandler<SequentialStep>
    {
        protected override bool? HandleStep(SequentialStep step, StateTable state, IStepHandler root)
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