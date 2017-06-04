namespace Soedeum.Dotnet.Library.Collections
{
    public interface ILookaheadScanner<T> : IScanner<T>
    {
        // Peek() = Peek(0);
        T Peek(int lookahead);

        // IsEnd = PeekIsEnd(0)
        bool PeekIsEnd(int lookahead);
    }
}