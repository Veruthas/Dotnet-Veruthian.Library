using Veruthian.Library.Readers;
using Veruthian.Library.Types;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Operations.Analyzers
{
    public abstract class BaseLookaheadOperation<TState, TReader, T> : BaseReadOperation<TState, TReader, T>
        where TState : Has<TReader>
        where TReader : ILookaheadReader<T>
    {
        protected readonly int lookahead;

        protected BaseLookaheadOperation(int lookahead)
        {
            ExceptionHelper.VerifyPositive(lookahead);

            this.lookahead = lookahead;
        }

    }
}