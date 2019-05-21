using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public interface IVector<A, T> : ILookup<A, T>
        where A : ISequential<A>
    {
        A Start { get; }

        A GetAddress(Number offset);
    }

    public interface IVector<T> : IVector<Number, T>
    {

    }
}