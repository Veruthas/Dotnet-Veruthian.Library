using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Operations
{
    public interface IOperation<TState> : IEnumerable<IOperation<TState>>
    {
        string Description { get; }

        bool Perform(TState state, IOperationTracer<TState> tracer = null);


        IOperation<TState> GetSubOperation(int index);

        int Count { get; }
    }
}