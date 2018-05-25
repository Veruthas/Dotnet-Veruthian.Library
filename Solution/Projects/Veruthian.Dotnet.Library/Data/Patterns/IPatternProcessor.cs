using Veruthian.Dotnet.Library.Data.Readers;

namespace Veruthian.Dotnet.Library.Data.Patterns
{
    public interface IPatternProcessor<T, TReader, TState>
        where TReader : IReader<T>
    {
        void OnDescend(IPattern<T, TReader, TState> pattern, TReader reader, TState state);

        void OnAscend(IPattern<T, TReader, TState> pattern, TReader reader, TState state, bool successful);
    }
}