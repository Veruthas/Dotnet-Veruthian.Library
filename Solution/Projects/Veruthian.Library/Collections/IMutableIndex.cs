namespace Veruthian.Library.Collections
{
    public interface IMutableIndex<A, V> : IIndex<A, V>, IMutableLookup<A, V>
    {

    }

    public interface IMutableIndex<V> : IMutableIndex<int, V>, IIndex<V>
    {
        
    }
}