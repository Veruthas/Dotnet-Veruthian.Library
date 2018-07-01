using Veruthian.Dotnet.Library.Readers;

namespace Veruthian.Dotnet.Library.Patterns
{
    public class DynamicCheckOperation<TState, TReader, T> : CheckOperation<TState, TReader, T>
        where TState : Has<TReader>
        where TReader : ILookaheadReader<T>
    {
        MatchFunction<T> match;

        string description;


        protected DynamicCheckOperation(MatchFunction<T> match, int lookahead, string description = null)
            : base(lookahead)
        {
            this.match = match;
            this.description = description;
        }


        public override string Description => description ?? "DynamicCheckOperation";

        protected override bool Match(T item) => match(item);
    }
}