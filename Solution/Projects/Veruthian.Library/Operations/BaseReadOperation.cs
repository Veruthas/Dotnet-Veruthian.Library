using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations
{
    public abstract class BaseReadOperation<TState, T> : BaseSimpleOperation<TState>
        where TState : Has<IReader<T>>
    {
        protected override bool DoAction(TState state, ITracer<TState> tracer = null)
        {
            state.Get(out var reader);

            var value = reader.Peek();

            var result = Match(value);

            if (result) reader.Read();

            return result;
        }

        protected abstract bool Match(T value);
    }
}