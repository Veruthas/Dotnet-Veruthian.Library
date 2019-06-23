using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps.Actions
{
    public class BooleanStep : BaseStep, IActionStep
    {
        public BooleanStep(bool? value, bool atStart = false, bool atComplete = false)
        {
            this.Value = value;

            this.AtStart = atStart;

            this.AtComplete = atComplete;
        }

        public bool? Value { get; }

        public bool AtStart { get; }

        public bool AtComplete { get; }

        public override string Description => $"Boolean<{(Value == null ? "null" : Value.ToString())}>";

        public bool? Act(bool? state, bool completed)
        {
            if (!completed)
            {
                if (AtStart)
                    return Value;
            }
            else
            {
                if (AtComplete)
                    return Value;
            }

            return state;
        }
    }
}