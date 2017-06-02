namespace Soedeum.Dotnet.Library.Collections
{
    public interface IScanner<T>
    {
        bool IsEnd { get; }

        int Position { get; }

        T Peek();

        T Consume();
    }
}