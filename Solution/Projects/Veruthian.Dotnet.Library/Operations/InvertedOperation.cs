using System;

namespace Veruthian.Dotnet.Library.Operations
{
    public class InvertedOperation<TState> : NestedOperation<TState>
    {
        public InvertedOperation(IOperation<TState> operation) : base(operation) { }

        public override string Description => "Invert";

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null) => !Operation.Perform(state, tracer);
        
    }
}