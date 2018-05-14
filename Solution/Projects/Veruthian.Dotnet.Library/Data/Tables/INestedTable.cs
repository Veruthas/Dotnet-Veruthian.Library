using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public interface INestedTable<TKey, TValue, TTable> : ITable<TKey, TValue>
        where TTable : ITable<TKey, TValue>
    {
        TTable InnerTable { get; }

        TTable OuterTable { get; }
    }
}