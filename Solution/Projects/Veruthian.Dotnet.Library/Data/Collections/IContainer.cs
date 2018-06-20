using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public interface IContainer<T>
    {
        int Count { get; }

        bool Contains(T value);

        IEnumerable<T> Values { get; }
    }
}