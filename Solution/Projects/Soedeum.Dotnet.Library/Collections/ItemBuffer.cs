using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class ItemBuffer<T> : IBuffer<T, T[]>
    {
        bool isBuffering;

        List<T> buffer = new List<T>();


        public bool IsBuffering => isBuffering;

        public void Capture()
        {
            isBuffering = true;
            buffer.Clear();
        }

        public void Add(T value)
        {
            if (IsBuffering)
                buffer.Add(value);
        }

        public T[] Extract()
        {
            return buffer.ToArray();
        }

        public void Release()
        {
            isBuffering = false;
            buffer.Clear();
        }

        public void Release(int amount)
        {
            if (amount > buffer.Count)
                Release();
            else
                buffer.RemoveRange(buffer.Count - amount, amount);
        }
    }
}