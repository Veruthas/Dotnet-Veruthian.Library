using System;
using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public interface IReader<out T> : IDisposable
    {
        T Current { get; }

        int Position { get; }

        bool IsEnd { get; }

        void Advance();

        void Advance(int amount);
    }
}