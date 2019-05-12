using Veruthian.Library.Readers;
using Veruthian.Library.Readers.Extensions;
using Veruthian.Library.Types;

namespace Veruthian.Library.Steps.Handlers
{
    public class ReaderStepHandler<TState, T> : MatchStepHandler<TState, T>
        where TState : Has<IReader<T>>
    {
        protected override T GetCurrent(TState state)
        {
            state.Get(out var reader);

            return reader.Current;
        }

        protected override bool TryAdvance(TState state)
        {
            state.Get(out var reader);

            return reader.TryAdvance();
        }
    }
}