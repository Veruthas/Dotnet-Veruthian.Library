using System;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class OptionalOperation<TState> : NestedOperation<TState>
    {
        public OptionalOperation(IOperation<TState> operation) : base(operation) { }

        public override string Description => "Optional";
        
        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null)
        {
            operation.Perform(state, tracer);

            return true;
        }
    }
}