using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class BufferedReader<T> : ReaderAdapater<T>, IBuffer<T>
    {
        protected BufferedReader(IReader<T> reader, IBuffer<T> buffer)
            : base(reader)
        {
            this.Buffer = buffer;
        }

        protected IBuffer<T> Buffer { get; }


        public int BufferedCount => Buffer.BufferedCount;


        void IBuffer<T>.AddToBuffer(T value) => throw new InvalidOperationException("Buffer is being managed by reader.");

        void IBuffer<T>.AddToBuffer(IEnumerable<T> values) => throw new InvalidOperationException("Buffer is being managed by reader.");

        void IBuffer<T>.ClearBuffer() => throw new InvalidOperationException("Buffer is being managed by reader.");

        void IBuffer<T>.RollbackBuffer(int amount) => throw new InvalidOperationException("Buffer is being managed by reader.");


        public bool IsBuffering => Buffer.IsBuffering;

        public void StartBuffering() => Buffer.StartBuffering();

        public void StopBuffering()
        {
            Buffer.ClearBuffer();

            Buffer.StopBuffering();
        }

        public void SetBufferedItem(int index, T value) => Buffer.SetBufferedItem(index, value);

        public T GetBufferedItem(int index) => Buffer.GetBufferedItem(index);

        public IEnumerable<T> GetBuffered(int index, int amount) => Buffer.GetBuffered(index, amount);

        public IEnumerable<T> GetBuffered() => Buffer.GetBuffered();


        public override T Read()
        {
            var item = Reader.Read();

            if (Buffer.IsBuffering)
                Buffer.AddToBuffer(item);

            return item;
        }
        
        public override void Skip(int amount)
        {
            if (Buffer.IsBuffering)
                for (int i = 0; i < amount;i++)                
                    Read();
            else
                base.Skip(amount);
        }
    }
}