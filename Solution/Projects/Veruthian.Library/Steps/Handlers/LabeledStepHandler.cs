using System;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps.Handlers
{
    public class LabeledStepHandler : StepHandler<LabeledStep>
    {        
        protected override bool? HandleStep(LabeledStep step, StateTable state, IStepHandler root)
        {
            return root.Handle(step.Step, state);
        }
    }
}