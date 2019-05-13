namespace Veruthian.Library.Collections
{
    public interface IVectorLookup<L, V, T> : ILookup<L, T>
    {
        T GetByVectorAddress(V address);

        T this[V address] { get; }

        V Start { get; }
    }
}