namespace Veruthian.Dotnet.Library.Data.Collections
{
    public interface IResizeableLookup<TKey, TValue> : ISettableLookup<TKey, TValue>
    {
        void Insert(TKey key, TValue value);

        void Remove(TKey key);

        void Clear();
    }
}