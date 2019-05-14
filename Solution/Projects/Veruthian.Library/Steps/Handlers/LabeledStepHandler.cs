using System;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps.Handlers
{
    public class LabeledStepHandler<TState> : StepHandler<TState, LabeledStep>
    {        
        protected override bool? HandleStep(LabeledStep step, TState state, IStepHandler<TState> root)
        {
            return root.Handle(step.Step, state);
        }
    }
}