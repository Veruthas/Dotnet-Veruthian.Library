using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data
{
    public interface IBuffer<T>
    {
        int BufferedCount { get; }

        T GetBufferedItem(int index);

        void SetBufferedItem(int index, T value);

        void AddToBuffer(T value);

        void AddToBuffer(IEnumerable<T> values);

        void ClearBuffer();

        void RollbackBuffer(int amount);

        
        IEnumerable<T> GetBuffered();

        IEnumerable<T> GetBuffered(int index, int amount);


        bool IsBuffering { get; }

        void StartBuffering();

        void StopBuffering();
    }
}