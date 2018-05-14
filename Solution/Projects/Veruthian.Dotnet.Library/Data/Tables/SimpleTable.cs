using System.Collections.Generic;
using Veruthian.Dotnet.Library.Data.Enumeration;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public class SimpleTable<TKey, TValue> : TableBase<TKey, TValue>
    {
        public override int BaseCount => 0;

        public override IEnumerable<KeyValuePair<TKey, TValue>> GetBasePairs() => EmptyEnumerable<KeyValuePair<TKey, TValue>>.Default;

        public override IEnumerable<ITable<TKey, TValue>> GetBaseTables() => EmptyEnumerable<ITable<TKey, TValue>>.Default;

        public override bool HasBaseKey(TKey key) => false;

        public override bool ResolveBase(TKey key, out TValue value)
        {
            value = default(TValue);

            return false;
        }

        public override void SetBase(TKey key, TValue value)
        {
            throw new System.NullReferenceException("No base table");
        }

        protected override IEnumerable<KeyValuePair<TKey, TValue>> GetUniquePairs(bool includeLocal)
        {
            if (includeLocal)
                return GetLocalPairs();
            else
                return GetBasePairs();
        }
    }
}