using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public class ParalleledTable<TKey, TValue> : Table<TKey, TValue>, IParalleledTable<TKey, TValue, Table<TKey, TValue>>
    {
        List<Table<TKey, TValue>> tables = new List<Table<TKey, TValue>>();


        public ParalleledTable() { }

        public ParalleledTable(params Table<TKey, TValue>[] tables) => this.tables.AddRange(tables);

        public ParalleledTable(IEnumerable<Table<TKey, TValue>> tables) => this.tables.AddRange(tables);


        public int TableCount => tables.Count;

        public Table<TKey, TValue> GetTable(int index) => tables[index];

        IEnumerable<Table<TKey, TValue>> IParalleledTable<TKey, TValue, Table<TKey, TValue>>.GetTables() => Tables;

        public List<Table<TKey, TValue>> Tables => tables;


        public override int Count
        {
            get
            {
                int total = 0;

                foreach (var table in tables)
                    total += table.Count;

                return total;
            }
        }

        public override bool HasKey(TKey key)
        {
            foreach (var table in tables)
            {
                if (table.HasKey(key))
                    return true;
            }

            return false;
        }

        public override bool Get(TKey key, out TValue value)
        {
            foreach (var table in tables)
            {
                if (table.Get(key, out value))
                    return true;
            }

            value = default(TValue);

            return false;
        }

        public override void Set(TKey key, TValue value)
        {
            if (tables.Count == 0)
                throw new System.NullReferenceException("No tables in list.");

            foreach (var table in tables)
            {
                if (table.HasKey(key))
                {
                    table.Set(key, value);
                    return;
                }
            }

            tables[0].Set(key, value);
        }

        public override IEnumerable<KeyValuePair<TKey, TValue>> GetPairs()
        {
            foreach (var table in tables)
                foreach (var pair in table.GetPairs())
                    yield return pair;
        }
    }
}