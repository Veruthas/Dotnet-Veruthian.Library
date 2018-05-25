using Veruthian.Dotnet.Library.Data.Readers;

namespace Veruthian.Dotnet.Library.Data.Patterns
{
    public interface IPattern<T, TReader, TState>
        where TReader : IReader<T>
    {
        bool Follow(IPatternProcessor<T, TReader, TState> processor, TReader reader, TState state);
    }

    public interface ILookaheadPattern<T, TReader, TState> : IPattern<T, TReader, TState>
        where TReader : ILookaheadReader<T>
    {
        bool Check(IPatternProcessor<T, TReader, TState> processor, int index, TReader reader, TState state);
    }
}