namespace Veruthian.Library.Collections
{
    public interface IOrderedVector<A, V> : IVector<A, V>, IExpandableContainer<V>
    {
        void RemoveBy(A address);
    }

    public interface IOrderedVector<V> : IOrderedVector<int, V>, IVector<V>
    {

    }
}