using Veruthian.Dotnet.Library.Data.Operations;
using Veruthian.Dotnet.Library.Data.Readers;

namespace Veruthian.Dotnet.Library.Data.Patterns
{
    public abstract class MatchOperation<TState, TReader, T> : SimpleOperation<TState>
        where TState : HasType<TReader>
        where TReader : IReader<T>
    {

        bool readOnMatch;

        protected MatchOperation(bool readOnMatch = true) => this.readOnMatch = readOnMatch;

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null)
        {
            state.Get(out TReader reader);

            var item = reader.Peek();

            bool result = Match(item);

            return result;
        }

        protected abstract bool Match(T item);
    }
}