using System;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public abstract class NestedOperation<TState> : Operation<TState>, INestedOperation<TState>
    {
        protected readonly IOperation<TState> operation;

        public NestedOperation(IOperation<TState> operation)
        {
            if (operation == null)
                throw new ArgumentNullException("Operation cannot be null!");

            this.operation = operation;
        }

        public IOperation<TState> Operation => operation;
    }
}