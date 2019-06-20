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


        public const string TypeName = "Boolean";

        public override string Type => TypeName;


        public bool? Value { get; }

        public bool AtStart { get; }

        public bool AtComplete { get; }

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