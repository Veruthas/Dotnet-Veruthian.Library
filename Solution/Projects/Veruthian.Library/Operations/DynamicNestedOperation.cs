using System;

namespace Veruthian.Library.Operations
{
    public delegate bool NestedOperationAction<TState>(IOperation<TState> operation, TState state, IOperationTracer<TState> tracer = null);

    public class DynamicNestedOperation<TState> : NestedOperation<TState>
    {
        NestedOperationAction<TState> action;

        string description;


        public DynamicNestedOperation(IOperation<TState> operation, NestedOperationAction<TState> action, string description = null)
            : base(operation)
        {            
            this.action = action ?? ((o, s, t) => false);

            this.description = description;
        }

        public override string Description => description ?? "DynamicNestedOperation";

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null) => action(Operation, state, tracer);
    }
}