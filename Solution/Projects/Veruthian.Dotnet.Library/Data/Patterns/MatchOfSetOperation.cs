using Veruthian.Dotnet.Library.Data.Ranges;
using Veruthian.Dotnet.Library.Data.Readers;

namespace Veruthian.Dotnet.Library.Data.Patterns
{
    public class MatchOfSetOperation<TState, TReader, T> : MatchOperation<TState, TReader, T>
        where TState : HasType<TReader>
        where TReader : IReader<T>
        where T : ISequential<T>, new()
    {
        RangeSet<T> set;

        protected MatchOfSetOperation(RangeSet<T> set, bool readOnMatch = true) : base(readOnMatch) => this.set = set;

        public override string Description => $"MatchOfSet({set.ToString()})";

        protected override bool Match(T item) => set.Contains(item);
    }
}