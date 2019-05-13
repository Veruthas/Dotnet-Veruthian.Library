namespace Veruthian.Library.Collections
{
    public interface IVector<A, T> : ILookup<A, T>
    {
        A Start { get; }
    }

    public interface IVector<T> : IVector<int, T>
    {

    }
}