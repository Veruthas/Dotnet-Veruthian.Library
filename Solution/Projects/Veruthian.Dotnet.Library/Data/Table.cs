using System.Collections.Generic;
using Veruthian.Dotnet.Library.Data.Enumeration;

namespace Veruthian.Dotnet.Library.Data
{
    public class Table<TKey, TValue>
    {
        Dictionary<TKey, TValue> items = new Dictionary<TKey, TValue>();

        Table<TKey, TValue> baseTable;


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
        public Table<TKey, TValue> BaseTable { get => baseTable; set => baseTable = value; }

        public int BaseCount => baseTable != null ? baseTable.Count : 0;

        public bool ResolveBase(TKey key, out TValue value)
        {
            if (baseTable != null)
            {
                return baseTable.Resolve(key, out value);
            }
            else
            {
                value = default(TValue);
                return false;
            }
        }

        public void SetBase(TKey key, TValue value)
        {
            if (baseTable != null)
                baseTable.Set(key, value);
            else
                SetLocal(key, value);
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> GetBasePairs() => baseTable != null ? baseTable.GetPairs() : EmptyEnumerable<KeyValuePair<TKey, TValue>>.Default;

        public IEnumerable<KeyValuePair<TKey, TValue>> GetUniqueBasePairs() => GetUniquePairs(false);

        public bool HasBaseKey(TKey key) => baseTable != null ? baseTable.HasKey(key) : false;



        protected IEnumerable<KeyValuePair<TKey, TValue>> GetUniquePairs(bool includeLocal)
        {
            HashSet<TKey> set = new HashSet<TKey>();

            if (includeLocal)
            {
                foreach (var pair in GetLocalPairs())
                {
                    set.Add(pair.Key);

                    yield return pair;
                }
            }

            foreach (var pair in GetBasePairs())
            {
                if (!set.Contains(pair.Key))
                {
                    set.Add(pair.Key);

                    yield return pair;
                }
            }
        }
    }
}