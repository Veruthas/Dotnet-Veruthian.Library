namespace Veruthian.Library.Collections
{
    public interface IExpandableLookup<A, T> : ILookup<A, T>
    {
        void Insert(A address, T value);        

        void RemoveBy(A address);

        void Clear();
    }
}