using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public interface IOrderedVector<A, T> : IVector<A, T>, IResizableContainer<T>
        where A : ISequential<A>
    {
        void RemoveBy(A address);
    }

    public interface IOrderedVector<T> : IOrderedVector<Number, T>, IVector<T>
    {

    }
}