namespace Veruthian.Dotnet.Library.Data.Collections
{
    public interface ISettableLookup<TKey, TValue> : ILookup<TKey, TValue>
    {
        new TValue this[TKey key] { get; set; }
    }

}