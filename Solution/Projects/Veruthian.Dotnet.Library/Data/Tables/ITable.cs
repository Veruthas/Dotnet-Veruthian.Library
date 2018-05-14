using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public interface ITable<TKey, TValue>
    {
        // Recursive
        int Count { get; }

        bool Resolve(TKey key, out TValue value);

        void Set(TKey key, TValue value);

        IEnumerable<KeyValuePair<TKey, TValue>> GetPairs();

        IEnumerable<KeyValuePair<TKey, TValue>> GetUniquePairs();

        bool HasKey(TKey key);


        // Local
        int LocalCount { get; }

        bool ResolveLocal(TKey key, out TValue value);

        void SetLocal(TKey key, TValue value);

        IEnumerable<KeyValuePair<TKey, TValue>> GetLocalPairs();

        bool HasLocalKey(TKey key);


        // Base
        IEnumerable<ITable<TKey, TValue>> GetBaseTables();

        int BaseCount { get; }

        bool ResolveBase(TKey key, out TValue value);

        void SetBase(TKey key, TValue value);

        IEnumerable<KeyValuePair<TKey, TValue>> GetBasePairs();

        IEnumerable<KeyValuePair<TKey, TValue>> GetUniqueBasePairs();

        bool HasBaseKey(TKey key);
    }
}