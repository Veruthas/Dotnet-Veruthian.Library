using System;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class InvertedOperation<TState> : NestedOperation<TState>
    {
        public InvertedOperation(IOperation<TState> operation) : base(operation) { }

        public override string Name => "Invert";

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null) => !operation.Perform(state, tracer);
        
    }
}