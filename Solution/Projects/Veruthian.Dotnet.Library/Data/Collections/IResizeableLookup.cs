namespace Veruthian.Dotnet.Library.Data.Collections
{
    public interface IResizeableLookup<TKey, TValue> : ISettableLookup<TKey, TValue>
    {
        void Insert(TKey key, TValue value);

        void RemoveBy(TKey key);

        void Remove(TValue value);

        void Clear();
    }
}