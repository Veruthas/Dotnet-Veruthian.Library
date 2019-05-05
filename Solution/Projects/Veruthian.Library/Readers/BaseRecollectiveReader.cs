using System;
using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public class BaseRecollectiveReader<T, K, D> : BaseSpeculativeReader<T>, IRecollectiveReader<T, K, D>
    {
        protected struct Memory
        {
            private int StoredLength { get; }

            public bool Success => Length >= 0;

            public int Length => Math.Abs(StoredLength);

            public D Data { get; }


            public Memory(bool success, int length, D data)
            {
                this.StoredLength = success ? Math.Abs(length) : -Math.Abs(length);

                this.Data = data;
            }


            public static implicit operator (bool success, int Length, D Data)(Memory value) => (value.Success, value.Length, value.Data);

            public static implicit operator Memory((bool Success, int Length, D Data) value) => new Memory(value.Success, value.Length, value.Data);
        }

        SortedList<int, Dictionary<K, Memory>> memories = new SortedList<int, Dictionary<K, Memory>>();


        public void StoreProgress(K key, bool success, D data = default)
        {
            if (!memories.TryGetValue(MarkPosition, out var progresses))
            {
                memories.Add(MarkPosition, progresses = new Dictionary<K, Memory>());
            }

            progresses.Add(key, (success, Position - MarkPosition, data));
        }

        public (bool? success, int Length, D Data) RecallProgress(K key)
        {
            if (memories.TryGetValue(Position, out var progresses))
            {
                if (progresses.TryGetValue(key, out var progress))
                {
                    return (progress.Success, progress.Length, progress.Data);
                }
            }

            return (null, 0, default(D));
        }


        protected override void Reset()
        {
            base.Reset();

            memories.Clear();            
        }
    }
}