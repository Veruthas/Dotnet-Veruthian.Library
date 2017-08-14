using Soedeum.Dotnet.Library.Data.Readers;

namespace Soedeum.Dotnet.Library.Data.Patterns
{
    public interface IMatcher<T, TReader, TState>
        where TReader : IReader<T>
    {
        bool Match(TReader reader, TState state);
    }
}