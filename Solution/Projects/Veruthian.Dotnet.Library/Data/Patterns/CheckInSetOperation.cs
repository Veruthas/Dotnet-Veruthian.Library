using Veruthian.Dotnet.Library.Data.Ranges;
using Veruthian.Dotnet.Library.Data.Readers;

namespace Veruthian.Dotnet.Library.Data.Patterns
{
    public class CheckInSetOperation<T, TReader> : CheckOperation<T, TReader>
        where TReader : ILookaheadReader<T>
        where T : IOrderable<T>, new()
    {
        RangeSet<T> set;

        public CheckInSetOperation(RangeSet<T> set, int lookahead)
            : base(lookahead)
        {
            this.set = set;
        }

        public RangeSet<T> Set => set;

        public string Name => "CheckInSet";

        public override string Description => $"{Name}({(set == null ? "<NULL>" : set.ToString())})";

        protected override bool Match(T item) => this.set.Contains(item);
    }
}