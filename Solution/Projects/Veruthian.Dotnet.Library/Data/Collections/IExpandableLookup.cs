namespace Veruthian.Dotnet.Library.Data.Collections
{
    public interface IExpandableLookup<TKey, TValue> : IMutableLookup<TKey, TValue>
    {
        void Insert(TKey key, TValue value);

        void RemoveBy(TKey key);

        void Clear();
    }
}