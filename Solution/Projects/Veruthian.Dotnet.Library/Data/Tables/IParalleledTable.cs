using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public interface IParalleledTable<TKey, TValue, TTable> : ITable<TKey, TValue>
        where TTable : ITable<TKey, TValue>
    {
        int TableCount { get; }

        TTable GetTable(int index);

        IEnumerable<TTable> GetTables();
    }
}