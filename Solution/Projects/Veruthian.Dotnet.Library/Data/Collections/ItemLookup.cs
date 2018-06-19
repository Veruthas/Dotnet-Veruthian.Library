using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class ItemLookup<TKey, TValue> : IResizeableLookup<TKey, TValue>
    {
        Dictionary<TKey, TValue> items;

        bool defaultable;


        public ItemLookup() { }

        public ItemLookup(bool defaultable) => this.defaultable = defaultable;


        public TValue this[TKey key]
        {
            get => TryGet(key, out var value) ? value : defaultable ? default(TValue) : throw new KeyNotFoundException($"{key?.ToString() ?? ""} is not define.");
            set
            {
                items[key] = value;
            }
        }

        TValue ILookup<TKey, TValue>.this[TKey key] => this[key];

        public bool IsDefaultable => defaultable;

        public int Count => items.Count;


        public IEnumerable<TKey> Keys => items.Keys;

        public IEnumerable<TValue> Values => items.Values;

        public IEnumerable<KeyValuePair<TKey, TValue>> Pairs => items;

        public void Clear() => items.Clear();

        public bool HasKey(TKey key) => items.ContainsKey(key);

        public void Insert(TKey key, TValue value) => items.Add(key, value);

        public void Remove(TKey key) => items.Remove(key);

        public bool TryGet(TKey key, out TValue value) => items.TryGetValue(key, out value);
    }
}