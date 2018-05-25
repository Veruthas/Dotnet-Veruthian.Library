using System;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public interface IReader<T> : IDisposable
    {
        bool IsEnd { get; }

        int Position { get; }

        T Peek();

        T Read();

        int Skip(int amount);
    }
}