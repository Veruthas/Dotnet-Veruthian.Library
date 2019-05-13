namespace Veruthian.Library.Collections
{
    public interface IIndex<A, V> : ILookup<A, V>
    {
        A Start { get; }
    }

    public interface IIndex<V> : IIndex<int, V>
    {

    }
}