using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations
{
    public abstract class BaseReaderOperation<TState, T> : BaseSimpleOperation<TState>
        where TState : Has<IReader<T>>
    {
        protected override bool DoAction(TState state, ITracer<TState> tracer = null)
        {
            state.Get(out var reader);

            return Process(reader);
        }

        protected abstract bool Process(IReader<T> reader);
    }
}