using Veruthian.Dotnet.Library.Data.Operations;
using Veruthian.Dotnet.Library.Data.Readers;

namespace Veruthian.Dotnet.Library.Data.Patterns
{
    public abstract class CheckOperation<T, TReader> : SimpleOperation<TReader>
        where TReader : ILookaheadReader<T>
    {
        int lookahead;

        public CheckOperation(int lookahead)
        {
            this.lookahead = lookahead;
        }

        public int Lookahead => lookahead;

        protected override bool DoAction(TReader state, IOperationTracer<TReader> tracer = null)
        {
            var item = state.Peek(lookahead);

            var result = Match(item);

            return result;
        }

        protected abstract bool Match(T item);
    }
}
