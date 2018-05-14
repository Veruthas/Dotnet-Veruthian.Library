using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public interface ITable<TKey, TValue>
    {
        int Count { get; }

        bool HasKey(TKey key);

        void Set(TKey key, TValue value);

        bool Get(TKey key, out TValue value);

        IEnumerable<KeyValuePair<TKey, TValue>> GetPairs();
    }
}