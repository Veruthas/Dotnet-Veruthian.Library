using System;

namespace Veruthian.Library.Steps
{
    public class ActionStep : BaseSimpleStep
    {
        Func<bool> action;

        string description;

        public ActionStep(Func<bool> action, string description = "")
        {
            this.action = action ?? (() => false);

            this.description = description;
        }

        public override string Description => description ?? "action";

        public Func<bool> Action;
    }
}