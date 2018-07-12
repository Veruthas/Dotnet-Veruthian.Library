using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Collections
{
    public interface ILookup<TKey, TValue> : IContainer<TValue>
    {        
        TValue this[TKey key] { get; }

        IEnumerable<TKey> Keys { get; }

        IEnumerable<KeyValuePair<TKey, TValue>> Pairs { get; }

        bool HasKey(TKey key);

        bool TryGet(TKey key, out TValue value);
    }
}