using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public class NestedTable<TKey, TValue> : Table<TKey, TValue>, INestedTable<TKey, TValue, Table<TKey, TValue>>
    {
        Table<TKey, TValue> lower;

        Table<TKey, TValue> upper;



        public NestedTable(Table<TKey, TValue> lower, Table<TKey, TValue> upper = null)
        {
            this.lower = lower;

            this.upper = upper;
        }


        public Table<TKey, TValue> LowerTable => lower;

        public Table<TKey, TValue> UpperTable => upper;


        public override int Count => lower.Count + (upper != null ? upper.Count : 0);

        public override bool HasKey(TKey key) => lower.HasKey(key) || (upper != null && upper.HasKey(key));

        public override bool Get(TKey key, out TValue value)
        {
            if (lower.Get(key, out value))
                return true;
            else if (upper != null && upper.Get(key, out value))
                return true;
            else
                return false;
        }

        public override void Set(TKey key, TValue value) => lower.Set(key, value);

        public override IEnumerable<KeyValuePair<TKey, TValue>> GetPairs()
        {
            foreach (var pair in lower.GetPairs())
            {
                yield return pair;
            }

            if (upper != null)
            {
                foreach (var pair in upper.GetPairs())
                {
                    yield return pair;
                }
            }
        }
    }
}