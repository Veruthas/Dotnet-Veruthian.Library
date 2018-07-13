namespace Veruthian.Dotnet.Library.Collections
{
    public interface IOrderedIndex<K, V> : IIndex<K, V>, IExpandableContainer<V>
    {
        void RemoveBy(K key);
    }
}