using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public interface IMutableVector<A, T> : IVector<A, T>, IMutableLookup<A, T>
        where A : ISequential<A>
    {

    }

    public interface IMutableVector<T> : IMutableVector<Number, T>, IVector<T>
    {

    }
}