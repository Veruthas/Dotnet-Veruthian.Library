using System;
using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public abstract class NestedOperation<TState> : Operation<TState>
    {
        protected readonly IOperation<TState> operation;

        public NestedOperation(IOperation<TState> operation)
        {
            if (operation == null)
                throw new ArgumentNullException("Operation cannot be null!");

            this.operation = operation;
        }


        public IOperation<TState> Operation => operation;


        public override int Count => 1;

        public override IOperation<TState> GetSubOperation(int index)
        {
            VerifyIndex(index);

            return operation;
        }

        public override IEnumerator<IOperation<TState>> GetEnumerator()
        {
            yield return operation;
        }
    }
}