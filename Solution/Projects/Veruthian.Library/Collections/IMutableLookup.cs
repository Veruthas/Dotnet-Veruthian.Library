namespace Veruthian.Library.Collections
{
    public interface IMutableLookup<A, V> : ILookup<A, V>
    {
        new V this[A address] { get; set; }

        bool TrySet(A address, V value);
    }
}