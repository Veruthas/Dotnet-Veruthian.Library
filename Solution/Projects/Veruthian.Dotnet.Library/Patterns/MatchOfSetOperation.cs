using Veruthian.Dotnet.Library.Numeric;
using Veruthian.Dotnet.Library.Numeric.Ranges;
using Veruthian.Dotnet.Library.Readers;

namespace Veruthian.Dotnet.Library.Patterns
{
    public class MatchOfSetOperation<TState, TReader, T> : MatchOperation<TState, TReader, T>
        where TState : Has<TReader>
        where TReader : IReader<T>
        where T : struct, ISequential<T>, IBounded<T>
    {
        RangeSet<T> set;

        protected MatchOfSetOperation(RangeSet<T> set, bool readOnMatch = true) : base(readOnMatch) => this.set = set;

        public override string Description => $"MatchOfSet({set.ToString()})";

        protected override bool Match(T item) => set.Contains(item);
    }
}