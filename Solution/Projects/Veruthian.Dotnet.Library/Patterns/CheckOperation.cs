using Veruthian.Dotnet.Library.Operations;
using Veruthian.Dotnet.Library.Readers;

namespace Veruthian.Dotnet.Library.Patterns
{
    public abstract class CheckOperation<TState, TReader, T> : SimpleOperation<TState>
        where TState : HasType<TReader>
        where TReader : ILookaheadReader<T>
    {
        int lookahead;

        protected CheckOperation(int lookahead) => this.lookahead = lookahead;

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null)
        {
            state.Get(out TReader reader);

            var item = reader.Peek(lookahead);

            bool result = Match(item);

            return result;
        }

        protected abstract bool Match(T item);

    }
}