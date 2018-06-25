using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Readers
{
    public interface IReader<out T> : IDisposable
    {
        bool IsEnd { get; }

        int Position { get; }

        T Peek();

        T Read();

        IEnumerable<T> Read(int amount, bool includeEnd = false);

        void Skip(int amount);
    }
}