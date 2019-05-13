namespace Veruthian.Library.Collections
{
    public interface IMutableVector<A, T> : IVector<A, T>, IMutableLookup<A, T>
    {

    }

    public interface IMutableVector<T> : IMutableVector<int, T>, IVector<T>
    {
        
    }
}