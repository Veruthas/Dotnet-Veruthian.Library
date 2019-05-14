using System;

namespace Veruthian.Library.Steps.Handlers
{
    public class ConditionalStepHandler : StepHandler<ConditionalStep>
    {
        protected override bool? HandleStep(ConditionalStep step, StateTable state, IStepHandler root)
        {
            var result = HandleSpeculation(step.Condition, state, root);

            if (result == step.Expecting)
            {
                if (step.HasThenStep)
                    return root.Handle(step.Then, state, root);
                else
                    return true;
            }
            else if (result == !step.Expecting)
            {
                if (step.HasElseStep)
                    return root.Handle(step.Else, state, root);
                else
                    return false;
            }
            else
            {
                return null;
            }
        }

        protected virtual bool? HandleSpeculation(IStep speculation, StateTable state, IStepHandler root)
        {
            OnSpeculationStarted(speculation, state);

            var result = root.Handle(speculation, state, root);

            OnSpeculationCompleted(speculation, state, result);

            return result;
        }


        protected virtual void OnSpeculationStarted(IStep speculation, StateTable state)
        {
            if (SpeculationStarted != null)
                SpeculationStarted(speculation, state);
        }

        protected virtual void OnSpeculationCompleted(IStep speculation, StateTable state, bool? result)
        {
            if (SpeculationCompleted != null)
                SpeculationCompleted(speculation, state, result);
        }


        public event Action<IStep, StateTable> SpeculationStarted;

        public event Action<IStep, StateTable, bool?> SpeculationCompleted;
    }
}