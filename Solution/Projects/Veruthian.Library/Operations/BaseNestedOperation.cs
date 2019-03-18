using System;

namespace Veruthian.Library.Operations
{
    public abstract class BaseNestedOperation<TState> : BaseOperation<TState>
    {
        readonly IOperation<TState> operation;

        public BaseNestedOperation(IOperation<TState> operation) => this.operation = operation;


        protected IOperation<TState> Operation => operation;


        protected override int Count => 1;

        protected override IOperation<TState> GetSubOperation(int verifiedIndex) => operation;
    }
}