namespace Soedeum.Dotnet.Library.Collections
{
    public interface ILookaheadScanner<T> : IScanner<T>
    {
        T Peek(int lookahead);
    }
}