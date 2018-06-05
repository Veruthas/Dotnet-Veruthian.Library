using Veruthian.Dotnet.Library.Data.Readers;

namespace Veruthian.Dotnet.Library.Data.Patterns
{
    public class CheckEqualsOperation<T, TReader> : CheckOperation<T, TReader>
        where TReader : ILookaheadReader<T>
    {
        T item;

        public CheckEqualsOperation(T item, int lookahead)
            : base(lookahead)
        {
            this.item = item;
        }

        public T Item => item;

        public string Name => "CheckEquals";

        public override string Description => $"{Name}({(item == null ? "<NULL>" : item.ToString())})";

        protected override bool Match(T item)
        {
            if (this.item == null)
                return item == null;
            else
                return this.item.Equals(item);
        }
    }
}