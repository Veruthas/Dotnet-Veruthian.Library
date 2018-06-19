using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class ItemLookup<TKey, TValue> : IResizeableLookup<TKey, TValue>
    {
        Dictionary<TKey, TValue> items;


        public TValue this[TKey key]
        {
            get => items[key];
            set => items[key] = value;
        }

        TValue ILookup<TKey, TValue>.this[TKey key] => this[key];

        public bool IsDefaultable => false;

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