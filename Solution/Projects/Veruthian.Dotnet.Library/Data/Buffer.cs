using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data
{
    public class Buffer<T> : IBuffer<T>
    {
        List<T> buffer = new List<T>();

        bool buffering;

        public Buffer()
        {
        }

        public int BufferedCount => buffer.Count;


        public IEnumerable<T> GetBuffered()
        {
            foreach (var item in buffer)
                yield return item;
        }

        public IEnumerable<T> GetBuffered(int index, int amount)
        {
            if (index < 0 || index > buffer.Count)
                throw new ArgumentOutOfRangeException("index");
            if (amount < 0 || index + amount > buffer.Count)
                throw new ArgumentOutOfRangeException("amount");

            for (int i = index; i < index + amount; i++)
                yield return buffer[i];
        }

        public T GetBufferedItem(int index) => buffer[index];

        public void SetBufferedItem(int index, T item) => buffer[index] = item;

        public void AddToBuffer(T item)
        {
            if (buffering)
                buffer.Add(item);
        }

        public void AddToBuffer(IEnumerable<T> items)
        {
            if (buffering)
                buffer.AddRange(items);
        }

        public void ClearBuffer()
        {
            buffer.Clear();
        }

        public void RollbackBuffer(int amount)
        {
            if (amount < 0)
                amount = buffer.Count - amount + 1;

            if (amount > buffer.Count)
                throw new ArgumentOutOfRangeException("amount");
            else
                buffer.RemoveRange(buffer.Count - amount, amount);
        }

        public bool IsBuffering => buffering;

        public void StartBuffering()
        {
            buffering = true;
        }

        public void StopBuffering()
        {
            buffering = false;
        }
    }
}