namespace Veruthian.Library.Collections
{
    public interface IMutableLookup<K, V> : ILookup<K, V>
    {
        new V this[K key] { get; set; }

        bool TrySet(K key, V value);
    }
}