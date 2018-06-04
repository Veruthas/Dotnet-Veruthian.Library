using System;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class ActionOperation<TState> : SimpleOperation<TState>
    {
        string description;

        Predicate<TState> action;

        public ActionOperation(Predicate<TState> action, string description = null)
        {
            this.action = action;

            this.description = description;
        }


        public override string Name => description ?? $"Action";

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null) => action(state);
    }
}