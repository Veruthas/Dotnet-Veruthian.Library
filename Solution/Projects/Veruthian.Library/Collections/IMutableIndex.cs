namespace Veruthian.Library.Collections
{
    public interface IMutableIndex<K, V> : IIndex<K, V>, IMutableLookup<K, V>
    {

    }

    public interface IMutableIndex<V> : IMutableIndex<int, V>, IIndex<V>
    {
        
    }
}