using System;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class ActionOperation<TState> : IOperation<TState>
    {
        string description;

        Predicate<TState> action;

        public ActionOperation(Predicate<TState> action, string description = null)
        {
            this.action = action;

            this.description = description;
        }


        public bool Perform(TState state) => action(state);

        public bool Perform(TState state, IOperationTracer<TState> tracer)
        {
            tracer.StartingOperation(this, state);

            bool result = action(state);

            tracer.FinishingOperation(this, state, result);
            
            return result;
        }


        public override string ToString() => description ?? $"Action {base.ToString()}";
    }
}