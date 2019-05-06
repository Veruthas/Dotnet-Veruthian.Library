using System;
using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public class BaseRecollectiveReader<T> : BaseSpeculativeReader<T>, IRecollectiveReader<T>
    {
        protected struct Memory
        {
            private int StoredLength { get; }

            public bool Success => Length >= 0;

            public int Length => Math.Abs(StoredLength);

            public object Data { get; }


            public Memory(bool success, int length, object data)
            {
                this.StoredLength = success ? Math.Abs(length) : -Math.Abs(length);

                this.Data = data;
            }


            public static implicit operator (bool success, int Length, object Data)(Memory value) => (value.Success, value.Length, value.Data);

            public static implicit operator Memory((bool Success, int Length, object Data) value) => new Memory(value.Success, value.Length, value.Data);
        }

        SortedList<int, Dictionary<object, Memory>> memories = new SortedList<int, Dictionary<object, Memory>>();


        public void StoreProgress(object key, bool success, object data = null)
        {
            if (!IsSpeculating)
                throw new InvalidOperationException("Cannot store progress when not speculating.");

            if (!memories.TryGetValue(MarkPosition, out var progresses))
            {
                memories.Add(MarkPosition, progresses = new Dictionary<object, Memory>());
            }

            progresses.Add(key, (success, Position - MarkPosition, data));
        }

        public (bool? Success, int Length, object Data) RecallProgress(object key)
        {
            if (memories.TryGetValue(Position, out var progresses))
            {
                if (progresses.TryGetValue(key, out var progress))
                {
                    return (progress.Success, progress.Length, progress.Data);
                }
            }

            return (null, 0, null);
        }


        protected override void Reset()
        {
            base.Reset();

            memories.Clear();            
        }
    }
}