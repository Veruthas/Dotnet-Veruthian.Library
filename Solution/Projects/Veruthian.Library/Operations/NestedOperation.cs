using System;

namespace Veruthian.Library.Operations
{
    public abstract class NestedOperation<TState> : Operation<TState>
    {
        readonly IOperation<TState> operation;

        public NestedOperation(IOperation<TState> operation)
        {
            if (operation == null)
                throw new ArgumentNullException("Operation cannot be null!");

            this.operation = operation;
        }


        protected IOperation<TState> Operation => operation;


        protected override int Count => 1;

        protected override IOperation<TState> GetSubOperation(int verifiedIndex) => operation;
    }
}