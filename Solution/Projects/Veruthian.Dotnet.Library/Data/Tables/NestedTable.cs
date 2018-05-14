using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public class NestedTable<TKey, TValue> : Table<TKey, TValue>, INestedTable<TKey, TValue, Table<TKey, TValue>>
    {
        Table<TKey, TValue> inner;

        Table<TKey, TValue> outer;



        public NestedTable(Table<TKey, TValue> inner, Table<TKey, TValue> outer = null)
        {
            this.inner = inner;

            this.outer = outer;
        }


        public Table<TKey, TValue> InnerTable => inner;

        public Table<TKey, TValue> OuterTable => outer;


        public override int Count => inner.Count + (outer != null ? outer.Count : 0);

        public override bool HasKey(TKey key) => inner.HasKey(key) || (outer != null && outer.HasKey(key));

        public override bool Get(TKey key, out TValue value)
        {
            throw new System.NotImplementedException();
        }

        public override void Set(TKey key, TValue value) => inner.Set(key, value);

        public override IEnumerable<KeyValuePair<TKey, TValue>> GetPairs()
        {
            foreach(var pair in inner.GetPairs())
            {
                yield return pair;
            }

            if (outer != null)
            {
                foreach (var pair in outer.GetPairs())
                {
                    yield return pair;
                }
            }
        }
    }
}