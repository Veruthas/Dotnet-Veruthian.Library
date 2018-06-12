using System;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public delegate bool OperationAction<TState>(TState state);
    public class DynamicOperation<TState> : SimpleOperation<TState>
    {
        OperationAction<TState> action;

        string description;


        public DynamicOperation(OperationAction<TState> action, string description = null)
        {
            this.action = action ?? ((s) => false);

            this.description = description;
        }


        public override string Description => description ?? $"DynamicOperation";

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null) => action(state);
    }
}