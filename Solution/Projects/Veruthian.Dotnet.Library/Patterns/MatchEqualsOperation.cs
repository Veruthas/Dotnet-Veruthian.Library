using Veruthian.Dotnet.Library.Readers;

namespace Veruthian.Dotnet.Library.Patterns
{
    public class MatchEqualsOperation<TState, TReader, T> : MatchOperation<TState, TReader, T>
        where TState : HasType<TReader>
        where TReader : IReader<T>
    {
        T expected;

        protected MatchEqualsOperation(T expected, bool readOnMatch = true)
            : base(readOnMatch)
        {
            this.expected = expected;
        }

        public override string Description => $"Match({expected?.ToString() ?? ""})";

        protected override bool Match(T item) => (expected?.Equals(item)).GetValueOrDefault();
    }
}