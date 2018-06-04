using System;
using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public abstract class NestedOperation<TState> : Operation<TState>, IParentOperation<TState>
    {
        protected readonly IOperation<TState> operation;

        public NestedOperation(IOperation<TState> operation)
        {
            if (operation == null)
                throw new ArgumentNullException("Operation cannot be null!");

            this.operation = operation;
        }


        public IOperation<TState> Operation => operation;


        IOperation<TState> IParentOperation<TState>.this[int index]
        {
            get
            {
                if (index == 0)
                    return operation;
                else
                    throw new IndexOutOfRangeException("index");
            }
        }

        int IParentOperation<TState>.Count => 1;

        IEnumerator<IOperation<TState>> IEnumerable<IOperation<TState>>.GetEnumerator()
        {
            yield return operation;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return operation;
        }
    }
}