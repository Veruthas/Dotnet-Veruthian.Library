using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public interface INestedTable<TKey, TValue, TTable> : ITable<TKey, TValue>
        where TTable : ITable<TKey, TValue>
    {
        TTable LowerTable { get; }

        TTable UpperTable { get; }
    }
}