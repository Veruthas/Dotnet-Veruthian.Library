using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public static class Tables
    {
        public static IEnumerable<KeyValuePair<TKey, TValue>> GetUniquePairs<TKey, TValue>(this ITable<TKey, TValue> table)
        {
            HashSet<TKey> set = new HashSet<TKey>();

            foreach (var pair in table.GetPairs())
            {
                if (!set.Contains(pair.Key))
                {
                    set.Add(pair.Key);

                    yield return pair;
                }
            }
        }

        public static int GetUniqueCount<TKey, TValue>(this ITable<TKey, TValue> table)
        {
            HashSet<TKey> set = new HashSet<TKey>();


            foreach (var pair in table.GetPairs())
            {
                if (!set.Contains(pair.Key))
                {
                    set.Add(pair.Key);
                }
            }

            return set.Count;
        }

        public static TValue Get<TKey, TValue>(this ITable<TKey, TValue> table, TKey key)
        {
            if (table.Get(key, out var value))
                return value;
            else
                throw new System.ArgumentException(string.Format("Table does not contain key: '{0}'", key?.ToString() ?? "<NULL>"));
        }

        public static SimpleTable<TKey, TValue> CreateTable<TKey, TValue>() => new SimpleTable<TKey, TValue>();



        public static NestedTable<TKey, TValue> NestAsUpper<TKey, TValue>(this Table<TKey, TValue> upper, Table<TKey, TValue> withLower = null)
        {
            if (withLower == null)
                withLower = CreateTable<TKey, TValue>();

            var nested = new NestedTable<TKey, TValue>(withLower, upper);

            return nested;
        }


        public static NestedTable<TKey, TValue> NestAsLower<TKey, TValue>(this Table<TKey, TValue> lower, Table<TKey, TValue> withUpper)
        {
            if (withUpper == null)
                withUpper = CreateTable<TKey, TValue>();

            var nested = new NestedTable<TKey, TValue>(lower, withUpper);

            return nested;
        }


        public static ParalleledTable<TKey, TValue> CreateTableList<TKey, TValue>(params Table<TKey, TValue>[] tables) => new ParalleledTable<TKey, TValue>(tables);

        public static ParalleledTable<TKey, TValue> CreateTableList<TKey, TValue>(IEnumerable<Table<TKey, TValue>> tables) => new ParalleledTable<TKey, TValue>(tables);
    }
}