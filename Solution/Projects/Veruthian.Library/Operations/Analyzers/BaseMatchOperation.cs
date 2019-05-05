using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations.Analyzers
{
    public abstract class BaseMatchOperation<TState, TReader, T> : BaseReaderOperation<TState, TReader, T>
        where TState : Has<TReader>
        where TReader : IReader<T>
    {
        protected override bool Process(IReader<T> reader)
        {
            if (reader.IsEnd) return false;

            var value = reader.Peek();

            var success = Match(value);

            if (success) reader.Read();

            return success;
        }

        protected abstract bool Match(T value);
    }
}