using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public interface ILookup<TKey, TValue>
    {
        TValue this[TKey key] { get; }

        int Count { get; }

        IEnumerable<TKey> Keys { get; }

        IEnumerable<TValue> Values { get; }

        IEnumerable<KeyValuePair<TKey, TValue>> Pairs { get; }

        bool TryGet(TKey key, out TValue value);
    }
}