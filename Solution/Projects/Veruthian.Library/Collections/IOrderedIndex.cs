namespace Veruthian.Library.Collections
{
    public interface IOrderedIndex<A, V> : IIndex<A, V>, IExpandableContainer<V>
    {
        void RemoveBy(A key);
    }

    public interface IOrderedIndex<V> : IOrderedIndex<int, V>, IIndex<V>
    {

    }
}