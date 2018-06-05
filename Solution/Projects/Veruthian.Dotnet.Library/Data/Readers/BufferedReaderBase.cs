using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public abstract class BufferedReaderBase<T> : IReader<T>, IBuffer<T>
    {
        protected abstract IReader<T> Reader { get; }

        protected abstract IBuffer<T> Buffer { get; }

        #region Reader

        public bool IsEnd => Reader.IsEnd;

        public int Position => Reader.Position;

        public void Dispose() => Reader.Dispose();

        public T Peek() => Reader.Peek();

        public T Read()
        {
            var item = Reader.Read();

            if (Buffer.IsBuffering)
                Buffer.AddToBuffer(item);

            return item;
        }

        public void Skip(int amount)
        {
            if (Buffer.IsBuffering)
            {
                for (int i = 0; i < amount; i++)
                {
                    Buffer.AddToBuffer(Reader.Read());
                }
            }
            else
            {
                Reader.Skip(amount);
            }
        }

        #endregion

        #region Buffer

        public int BufferedCount => Buffer.BufferedCount;

        public bool IsBuffering => Buffer.IsBuffering;


        public void AddToBuffer(T value) => Buffer.AddToBuffer(value);

        public void AddToBuffer(IEnumerable<T> values) => Buffer.AddToBuffer(values);

        public void ClearBuffer() => Buffer.ClearBuffer();

        public IEnumerable<T> GetBuffered() => Buffer.GetBuffered();

        public IEnumerable<T> GetBuffered(int index, int amount) => Buffer.GetBuffered(index, amount);

        public T GetBufferedItem(int index) => Buffer.GetBufferedItem(index);

        public void RollbackBuffer(int amount) => Buffer.RollbackBuffer(amount);

        public void SetBufferedItem(int index, T value) => SetBufferedItem(index, value);

        public void StartBuffering() => Buffer.StartBuffering();
        
        public void StopBuffering() => Buffer.StopBuffering();

        #endregion
    }
}