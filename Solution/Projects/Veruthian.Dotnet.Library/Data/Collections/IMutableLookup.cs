namespace Veruthian.Dotnet.Library.Data.Collections
{
    public interface IMutableLookup<TKey, TValue> : ILookup<TKey, TValue>
    {
        new TValue this[TKey key] { get; set; }

        bool TrySet(TKey key, TValue value);
    }
}