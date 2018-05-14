using System.Collections.Generic;
using Veruthian.Dotnet.Library.Data.Enumeration;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public class NestedTable<TKey, TValue> : TableBase<TKey, TValue>
    {
        ITable<TKey, TValue> baseTable;

        public NestedTable(ITable<TKey, TValue> baseTable = null)
        {
            this.baseTable = baseTable;
        }

        public override int BaseCount => baseTable == null ? 0 : baseTable.Count;

        public override IEnumerable<KeyValuePair<TKey, TValue>> GetBasePairs()
        {
            if (baseTable != null)
                return baseTable.GetPairs();
            else
                return EmptyEnumerable<KeyValuePair<TKey, TValue>>.Default;
        }

        public override IEnumerable<ITable<TKey, TValue>> GetBaseTables()
        {
            if (baseTable != null)
                yield return baseTable;
            else
                yield break;
        }

        public override bool HasBaseKey(TKey key) => (baseTable == null) ? false : baseTable.HasKey(key);


        public override bool ResolveBase(TKey key, out TValue value)
        {
            if (baseTable != null)
            {
                return baseTable.ResolveBase(key, out value);
            }
            else
            {
                value = default(TValue);

                return false;
            }
        }


        public override void SetBase(TKey key, TValue value)
        {
            if (baseTable != null)
                baseTable.SetLocal(key, value);
            else
                throw new System.NullReferenceException("No base table");
        }

        protected override IEnumerable<KeyValuePair<TKey, TValue>> GetUniquePairs(bool includeLocal)
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