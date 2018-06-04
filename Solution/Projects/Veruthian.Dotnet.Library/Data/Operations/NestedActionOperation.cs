using System;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class NestedActionOperation<TState> : NestedOperation<TState>
    {
        Func<IOperation<TState>, TState, IOperationTracer<TState>, bool> action;

        string description;


        public NestedActionOperation(IOperation<TState> operation,
            Func<IOperation<TState>, TState, IOperationTracer<TState>, bool> action,
            string description = null)
            : base(operation)
        {
            this.action = action;

            this.description = description;
        }

        public override string Description => description ?? "NestedAction";

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null) => action(operation, state, tracer);
    }
}