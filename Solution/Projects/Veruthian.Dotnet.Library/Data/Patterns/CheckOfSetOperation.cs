using Veruthian.Dotnet.Library.Data.Ranges;
using Veruthian.Dotnet.Library.Data.Readers;

namespace Veruthian.Dotnet.Library.Data.Patterns
{
    public class CheckOfSetOperation<TState, TReader, T> : CheckOperation<TState, TReader, T>
        where TState : HasType<TReader>
        where TReader : ILookaheadReader<T>
        where T : IOrderable<T>, new()
    {
        RangeSet<T> set;

        protected CheckOfSetOperation(RangeSet<T> set, int lookahead) : base(lookahead) => this.set = set;

        public override string Description => $"MatchOfSet({set.ToString()})";

        protected override bool Match(T item) => set.Contains(item);
    }
}