using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public interface IReader<T> : IDisposable
    {
        bool IsEnd { get; }

        int Position { get; }

        T Peek();

        T Read();

        void Read(int amount);


        event ReaderRead<T> ItemRead;
    }

    public delegate void ReaderRead<T>(IReader<T> reader, T item);    
}