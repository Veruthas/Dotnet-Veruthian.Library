using Veruthian.Dotnet.Library.Readers;

namespace Veruthian.Dotnet.Library.Patterns
{
    public class DynamicMatchOperation<TState, TReader, T> : MatchOperation<TState, TReader, T>
        where TState : HasType<TReader>
        where TReader : IReader<T>
    {
        MatchFunction<T> match;

        string description;


        protected DynamicMatchOperation(MatchFunction<T> match, bool readOnMatch = true, string description = null)
            : base(readOnMatch)
        {
            this.match = match;
            this.description = description;
        }


        public override string Description => description ?? "DynamicMatchOperation";

        protected override bool Match(T item) => match(item);
    }
}