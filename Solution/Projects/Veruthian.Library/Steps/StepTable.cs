using System;
using System.Collections.Generic;

namespace Veruthian.Library.Steps
{
    public class StepTable<S>
        where S : BaseContainedStep
    {
        private Func<string, S> newStep;

        private Dictionary<string, S> steps;

        public StepTable(Func<string, S> newStep)
        {
            this.newStep = newStep;

            this.steps = new Dictionary<string, S>();
        }


        public IStep this[string name]
        {
            get => GetStep(name);
            set
            {
                var step = GetStep(name);

                step.Step = value;
            }
        }

        public S GetStep(string name)
        {
            if (!steps.TryGetValue(name, out var step))
            {
                step = newStep(name);

                steps.Add(name, step);
            }

            return step;
        }
    }
}