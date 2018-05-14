using System.Collections.Generic;
using Veruthian.Dotnet.Library.Data.Enumeration;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public abstract class TableBase<TKey, TValue> : ITable<TKey, TValue>
    {
        Dictionary<TKey, TValue> items = new Dictionary<TKey, TValue>();

        // Recursive
        public int Count => LocalCount + BaseCount;

        public bool Resolve(TKey key, out TValue value) => ResolveLocal(key, out value) || ResolveBase(key, out value);

        public void Set(TKey key, TValue value) => SetLocal(key, value);

        public IEnumerable<KeyValuePair<TKey, TValue>> GetPairs()
        {
            foreach (var pair in GetLocalPairs())
                yield return pair;

            foreach (var pair in GetBasePairs())
                yield return pair;
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> GetUniquePairs() => GetUniquePairs(true);

        public bool HasKey(TKey key) => HasLocalKey(key) || HasBaseKey(key);


        // Local
        public int LocalCount => items.Count;

        public bool ResolveLocal(TKey key, out TValue value)
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

        public void SetLocal(TKey key, TValue value) => items[key] = value;

        public IEnumerable<KeyValuePair<TKey, TValue>> GetLocalPairs() => items;

        public bool HasLocalKey(TKey key) => items.ContainsKey(key);


        // Base
        public abstract IEnumerable<ITable<TKey, TValue>> GetBaseTables();

        public abstract int BaseCount { get; }

        public abstract bool ResolveBase(TKey key, out TValue value);

        public abstract void SetBase(TKey key, TValue value);

        public abstract IEnumerable<KeyValuePair<TKey, TValue>> GetBasePairs();

        public IEnumerable<KeyValuePair<TKey, TValue>> GetUniqueBasePairs() => GetUniquePairs(false);

        public abstract bool HasBaseKey(TKey key);



        protected abstract IEnumerable<KeyValuePair<TKey, TValue>> GetUniquePairs(bool includeLocal);
    }
}