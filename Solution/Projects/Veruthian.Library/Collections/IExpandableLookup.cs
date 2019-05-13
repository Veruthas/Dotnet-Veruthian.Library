namespace Veruthian.Library.Collections
{
    public interface IExpandableLookup<A, V> : ILookup<A, V>
    {
        void Insert(A address, V value);        

        void RemoveBy(A key);

        void Clear();
    }
}