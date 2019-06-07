namespace Veruthian.Library.Collections
{
    public interface IResizableLookup<A, T> : ILookup<A, T>
    {
        void Insert(A address, T value);        

        void RemoveBy(A address);

        void Clear();
    }
}