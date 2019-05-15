namespace Veruthian.Library.Collections
{
    public interface IOrderedVector<A, T> : IVector<A, T>, IResizableContainer<T>
    {
        void RemoveBy(A address);
    }

    public interface IOrderedVector<T> : IOrderedVector<int, T>, IVector<T>
    {

    }
}