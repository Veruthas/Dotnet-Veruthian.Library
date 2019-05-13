namespace Veruthian.Library.Collections
{
    public interface IMutableVector<A, V> : IVector<A, V>, IMutableLookup<A, V>
    {

    }

    public interface IMutableVector<V> : IMutableVector<int, V>, IVector<V>
    {
        
    }
}