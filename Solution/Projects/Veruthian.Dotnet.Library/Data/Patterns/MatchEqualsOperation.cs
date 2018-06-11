using Veruthian.Dotnet.Library.Data.Readers;

namespace Veruthian.Dotnet.Library.Data.Patterns
{
    public class MatchEqualsOperation<T, TReader> : MatchOperation<T, TReader>
        where TReader : IReader<T>
    {
        T item;

        public MatchEqualsOperation(T item, bool readOnMatch = true)
            : base(readOnMatch)
        {
            this.item = item;
        }

        public T Item => item;

        public string Name => "MatchEquals";

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