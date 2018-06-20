using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public abstract class ItemLookupBase<TKey, TValue> : ILookup<TKey, TValue>
    {
        public TValue this[TKey key] => TryGet(key, out var value) ? value : throw new KeyNotFoundException();

        public abstract int Count { get; }

        public abstract IEnumerable<TKey> Keys { get; }

        public abstract IEnumerable<TValue> Values { get; }

        public abstract IEnumerable<KeyValuePair<TKey, TValue>> Pairs { get; }
        

        public abstract bool Contains(TValue value);
        
        public abstract bool HasKey(TKey key);

        public abstract bool TryGet(TKey key, out TValue value);
    }
}