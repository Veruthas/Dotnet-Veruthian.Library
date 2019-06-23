using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps.Boolean
{
    public class BooleanStep : BaseStep, IActionStep
    {
        public BooleanStep(bool? value, bool atStart = false)
        {
            this.Value = value;

            this.AtStart = atStart;
        }

        public bool? Value { get; }

        public bool AtStart { get; }

        public override string Description => $"Boolean<{(Value == null ? "null" : Value.ToString())}>";

        public bool? Act(bool? state, bool completed)
        {
            if (!completed)
            {
                    return Value;
            }

            return state;
        }
    }
}