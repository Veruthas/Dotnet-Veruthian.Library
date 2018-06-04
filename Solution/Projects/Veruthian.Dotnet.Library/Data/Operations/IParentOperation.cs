using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public interface IParentOperation<TState> : IOperation<TState>, IEnumerable<IOperation<TState>>
    {
        IOperation<TState> this[int index] { get; }

        int Count { get; }
    }
}