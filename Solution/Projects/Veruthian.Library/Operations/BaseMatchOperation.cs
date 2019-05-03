using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations
{
    public abstract class BaseMatchOperation<TState, T> : BaseReaderOperation<TState, T>
        where TState : Has<IReader<T>>
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