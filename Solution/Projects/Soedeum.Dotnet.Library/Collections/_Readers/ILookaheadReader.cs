namespace Soedeum.Dotnet.Library.Collections
{
    public interface ILookaheadReader<T> : IReader<T>
    {
        T Peek(int lookahead);

        bool PeekIsEnd(int lookahead);
    }
}