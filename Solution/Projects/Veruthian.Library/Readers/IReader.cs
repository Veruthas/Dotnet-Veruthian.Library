using System;
using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public interface IReader<out T> : IDisposable
    {
        bool IsEnd { get; }

        int Position { get; }

        T Peek();

        T Read();

        void Skip(int amount);
    }
}