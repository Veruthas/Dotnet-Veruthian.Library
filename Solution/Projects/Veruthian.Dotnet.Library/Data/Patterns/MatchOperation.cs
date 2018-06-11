using Veruthian.Dotnet.Library.Data.Operations;
using Veruthian.Dotnet.Library.Data.Readers;

namespace Veruthian.Dotnet.Library.Data.Patterns
{
    public abstract class MatchOperation<T, TReader> : SimpleOperation<TReader>
        where TReader : IReader<T>
    {
        bool readOnMatch;

        public MatchOperation(bool readOnMatch)
        {
            this.readOnMatch = readOnMatch;
        }

        public bool ReadOnMatch => readOnMatch;

        protected override bool DoAction(TReader state, IOperationTracer<TReader> tracer = null)
        {
            var item = state.Peek();

            if (Match(item))
            {
                if (ReadOnMatch)
                    state.Read();

                return true;
            }

            return false;
        }

        protected abstract bool Match(T item);
    }
}