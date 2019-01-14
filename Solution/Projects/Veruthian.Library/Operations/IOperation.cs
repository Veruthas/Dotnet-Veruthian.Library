using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Operations
{
    public interface IOperation<TState> 
    {
        string Description { get; }

        bool Perform(TState state, IOperationTracer<TState> tracer = null);

        IIndex<int, IOperation<TState>> SubOperations { get; }
    }
}