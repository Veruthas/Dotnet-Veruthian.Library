namespace Veruthian.Dotnet.Library.Collections
{
    public interface IExpandableLookup<TKey, TValue> : ILookup<TKey, TValue>
    {        
        void Insert(TKey key, TValue value);        

        void RemoveBy(TKey key);

        void Clear();
    }
}