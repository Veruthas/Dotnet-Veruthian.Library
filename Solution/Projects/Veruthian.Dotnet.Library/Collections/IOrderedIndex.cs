namespace Veruthian.Dotnet.Library.Collections
{
    public interface IOrderedIndex<K, V> : IIndex<K, V>, IExpandableContainer<V>
    {
        void RemoveBy(K key);
    }

    public interface IOrderedIndex<V> : IOrderedIndex<int, V>, IIndex<V>
    {

    }
}