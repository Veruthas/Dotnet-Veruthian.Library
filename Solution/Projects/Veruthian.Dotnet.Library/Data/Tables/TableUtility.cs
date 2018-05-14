using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public static class TableUtility
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
    }
}