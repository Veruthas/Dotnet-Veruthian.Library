using Veruthian.Dotnet.Library.Data.Ranges;
using Veruthian.Dotnet.Library.Data.Readers;

namespace Veruthian.Dotnet.Library.Data.Patterns
{
    public class MatchInSetOperation<T, TReader> : MatchOperation<T, TReader>
        where TReader : IReader<T>
        where T : IOrderable<T>, new()
    {
        RangeSet<T> set;

        public MatchInSetOperation(RangeSet<T> set, bool readOnMatch = true)
            : base(readOnMatch)
        {
            this.set = set;
        }

        public RangeSet<T> Set => set;

        public string Name => "MatchInSet";

        public override string Description => $"{Name}({(set == null ? "<NULL>" : set.ToString())})";

        protected override bool Match(T item) => this.set.Contains(item);
    }
}