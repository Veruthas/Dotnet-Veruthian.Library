using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public interface IOperation<TState> : IEnumerable<IOperation<TState>>
    {
        string Name { get; }

        bool Perform(TState state, IOperationTracer<TState> tracer = null);


        IOperation<TState> GetSubOperation(int index);

        int Count { get; }
    }
}