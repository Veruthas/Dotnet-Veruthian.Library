using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations.Analyzers
{
    public abstract class BaseReadOperation<TState, TReader, T> : BaseSimpleOperation<TState>
        where TState : Has<TReader>
        where TReader: IReader<T>
    {
        protected override bool DoAction(TState state, ITracer<TState> tracer = null)
        {
            state.Get(out var reader);

            return Process(reader);
        }

        protected abstract bool Process(TReader reader);
    }
}