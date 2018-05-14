using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public class SimpleTable<TKey, TValue> : Table<TKey, TValue>
    {
        Dictionary<TKey, TValue> items = new Dictionary<TKey, TValue>();


        public override int Count => items.Count;

        public override bool HasKey(TKey key) => items.ContainsKey(key);

        public override bool Get(TKey key, out TValue value)
        {
            if (items.ContainsKey(key))
            {
                value = items[key];

                return true;
            }
            else
            {
                value = default(TValue);

                return false;
            }
        }

        public override void Set(TKey key, TValue value)
        {
            items[key] = value;
        }

        public override IEnumerable<KeyValuePair<TKey, TValue>> GetPairs()
        {
            return items;
        }
    }
}