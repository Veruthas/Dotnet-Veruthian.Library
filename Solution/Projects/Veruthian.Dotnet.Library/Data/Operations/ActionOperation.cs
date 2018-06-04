using System;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class ActionOperation<TState> : SimpleOperation<TState>
    {
        Func<TState, bool> action;

        string description;


        public ActionOperation(Func<TState, bool> action, string description = null)
        {
            this.action = action;

            this.description = description;
        }


        public override string Description => description ?? $"Action";

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null) => action(state);
    }
}