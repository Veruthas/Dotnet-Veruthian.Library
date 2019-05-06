using Veruthian.Library.Readers;
using Veruthian.Library.Types;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Operations.Analyzers
{
    public abstract class BaseMatchAheadOperation<TState, TReader, T> : BaseLookaheadOperation<TState, TReader, T>
        where TState : Has<TReader>
        where TReader : ILookaheadReader<T>
    {
        protected BaseMatchAheadOperation(int lookahead) : base(lookahead) { }

        protected override bool Process(TReader reader)
        {
            if (reader.IsEnd) return false;

            var value = reader.Peek(lookahead);

            var success = Match(value);

            return success;
        }

        protected abstract bool Match(T value);
    }
}