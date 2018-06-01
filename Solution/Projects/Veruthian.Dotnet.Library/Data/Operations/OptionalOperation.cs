using System;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class OptionalOperation<TState> : IOperation<TState>
    {
        IOperation<TState> operation;

        public OptionalOperation(IOperation<TState> operation)
        {
            if (operation == null)
                throw new ArgumentNullException("Operation cannot be null!");
                
            this.operation = operation;
        }


        public IOperation<TState> Operation => operation;


        public bool Perform(TState state)
        {
            operation.Perform(state);

            return true;
        }

        public bool Perform(TState state, IOperationTracer<TState> tracer)
        {
            tracer.StartingOperation(operation, state);

            operation.Perform(state, tracer);

            tracer.FinishingOperation(operation, state, true);

            return true;
        }

        public override string ToString() => "[" + operation.ToString() + "]";
    }
}