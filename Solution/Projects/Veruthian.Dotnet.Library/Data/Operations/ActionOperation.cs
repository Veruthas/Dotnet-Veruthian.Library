using System;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class ActionOperation<TState> : Operation<TState>
    {
        string description;

        Predicate<TState> action;

        public ActionOperation(Predicate<TState> action, string description = null)
        {
            this.action = action;

            this.description = description;
        }


        public override string ToString() => description ?? $"Action {base.ToString()}";

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null) => action(state);
    }
}