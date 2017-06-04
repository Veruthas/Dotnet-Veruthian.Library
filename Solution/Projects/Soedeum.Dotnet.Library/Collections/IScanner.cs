using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public interface IScanner<T> : IDisposable
    {
        bool IsEnd { get; }

        int Position { get; }

        T Peek();

        T Read();

        void Read(int amount);
    }
}