using Veruthian.Library.Collections;

namespace Veruthian.Library.Operations
{
    public interface IOperation<TState>
    {
        string Description { get; }

        bool Perform(TState state, ITracer<TState> tracer = null);

        IIndex<IOperation<TState>> SubOperations { get; }
    }
}