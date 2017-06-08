namespace Soedeum.Dotnet.Library.Collections
{
    public interface ILookaheadReader<T> : IReader<T>
    {
        // Peek() = Peek(0);
        T Peek(int lookahead);

        // IsEnd = PeekIsEnd(0)
        bool PeekIsEnd(int lookahead);
    }
}