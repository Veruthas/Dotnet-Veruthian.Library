using Veruthian.Dotnet.Library.Readers;

namespace Veruthian.Dotnet.Library.Patterns
{
    public class CheckEqualsOperation<TState, TReader, T> : CheckOperation<TState, TReader, T>
        where TState : HasType<TReader>
        where TReader : ILookaheadReader<T>
    {
        T expected;

        protected CheckEqualsOperation(int lookahead, T expected)
            : base(lookahead)
        {
            this.expected = expected;
        }

        public override string Description => $"Match({expected?.ToString() ?? ""})";

        protected override bool Match(T item) => (expected?.Equals(item)).GetValueOrDefault();
    }
}