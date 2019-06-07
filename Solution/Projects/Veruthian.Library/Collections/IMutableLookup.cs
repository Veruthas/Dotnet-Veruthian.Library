namespace Veruthian.Library.Collections
{
    public interface IMutableLookup<A, T> : ILookup<A, T>
    {
        new T this[A address] { get; set; }

        bool TrySet(A address, T value);
    }
}