namespace Soedeum.Dotnet.Library.Data.Readers
{
    public interface ILookaheadReader<T> : IReader<T>
    {
        T Peek(int lookahead);

        bool PeekIsEnd(int lookahead);
    }
}