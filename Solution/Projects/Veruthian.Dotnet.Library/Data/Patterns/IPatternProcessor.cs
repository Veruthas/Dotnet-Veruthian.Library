using Veruthian.Dotnet.Library.Data.Readers;

namespace Veruthian.Dotnet.Library.Data.Patterns
{
    public interface IPatternProcessor<T, TReader>
        where TReader : IReader<T>
    {
        void OnDescend(IPattern pattern, TReader reader);

        void OnAscend(IPattern pattern, TReader reader, bool successful);
    }
}