namespace Veruthian.Dotnet.Library.Collections
{
    public interface IExpandableLookup<K, V> : ILookup<K, V>
    {
        void Insert(K key, V value);        

        void RemoveBy(K key);

        void Clear();
    }
}