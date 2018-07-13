using System.Collections.Generic;
using Veruthian.Dotnet.Library.Collections;

namespace Veruthian.Dotnet.Library.Operations
{
    public interface IOperation<TState> 
    {
        string Description { get; }

        bool Perform(TState state, IOperationTracer<TState> tracer = null);

        IIndex<int, IOperation<TState>> SubOperations { get; }
    }
}