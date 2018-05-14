using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public abstract class Table<TKey, TValue> : ITable<TKey, TValue>
    {
        public abstract int Count { get; }

        public abstract bool HasKey(TKey key);

        public abstract bool Get(TKey key, out TValue value);

        public abstract void Set(TKey key, TValue value);

        public abstract IEnumerable<KeyValuePair<TKey, TValue>> GetPairs();
    }
}