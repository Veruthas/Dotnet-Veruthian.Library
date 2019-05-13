namespace Veruthian.Library.Collections
{
    public interface IVector<A, V> : ILookup<A, V>
    {
        A Start { get; }
    }

    public interface IVector<V> : IVector<int, V>
    {

    }
}