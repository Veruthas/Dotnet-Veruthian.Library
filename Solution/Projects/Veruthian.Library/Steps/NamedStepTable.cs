using System;
using System.Collections.Generic;

namespace Veruthian.Library.Steps
{
    public class NamedStepTable<N>
        where N : BaseNamedStep
    {
        private Func<string, N> newStep;

        private Dictionary<string, N> steps;

        public NamedStepTable(Func<string, N> newStep)
        {
            this.newStep = newStep;

            this.steps = new Dictionary<string, N>();
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

        private N GetStep(string name)
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