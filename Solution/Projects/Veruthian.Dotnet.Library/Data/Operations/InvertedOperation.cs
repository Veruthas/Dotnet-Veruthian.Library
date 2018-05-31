using System;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class InvertedOperation<TState> : IOperation<TState>
    {
        IOperation<TState> operation;

        public InvertedOperation(IOperation<TState> operation)
        {
            if (operation == null)
                throw new ArgumentNullException("Operation cannot be null!");
                
            this.operation = operation;
        }

        public bool Perform(TState state)
        {
            bool result = operation.Perform(state);

            return !result;
        }

        public bool Perform(TState state, IOperationTracer<TState> tracer)
        {
            tracer.StartingOperation(operation, state);

            var result = operation.Perform(state, tracer);

            tracer.FinishingOperation(operation, state, !result);

            return !result;
        }

        public override string ToString() => "not (" + operation.ToString() + ")";
    }
}