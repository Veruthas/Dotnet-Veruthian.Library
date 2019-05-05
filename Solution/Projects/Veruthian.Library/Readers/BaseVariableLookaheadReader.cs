using System;
using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public abstract class BaseVariableLookaheadReader<T> : BaseLookaheadReader<T>
    {
        List<T> cache = new List<T>();

        int index;


        public BaseVariableLookaheadReader() { }


        protected List<T> Cache { get => cache; }

        protected int CacheIndex { get => index; set => index = value; }

        protected int CacheSize { get => cache.Count; }

        protected virtual bool CanReset { get => true; }


        protected override void Initialize()
        {
            cache.Clear();

            index = 0;

            Position = 0;
        }


        protected void EnsureIndex(int index)
        {
            int available = (CacheSize - index);

            // Prefetch (d + 1) items
            if (available <= 0)
                Prefetch(available + 1);
        }

        protected override void EnsureLookahead(int lookahead = 0) => EnsureIndex(index + lookahead);


        private void Prefetch(int amount)
        {
            if (!EndFound)
            {
                int lastPosition = CacheSize;

                for (int i = 0; i < amount; i++)
                {
                    bool success = GetNext(out T next);

                    if (success)
                    {
                        cache.Add(next);
                    }
                    else
                    {
                        EndPosition = lastPosition + i;

                    }
                }
            }
        }

        protected T RawPeekByIndex(int index)
        {
            // We don't want to add infinite end items, just return the cached end item
            if (EndFound && index >= cache.Count)
                return LastItem;
            else
                return cache[index];
        }

        protected override T RawPeek(int lookahead = 0) => RawPeekByIndex(index + lookahead);


        protected override void MoveNext()
        {
            if (!IsEnd)
            {
                Position++;

                index++;

                if (index == CacheSize && CanReset)
                    Reset();

                EnsureLookahead(1);
            }
        }

        protected override void TryPreload(int amount) => EnsureLookahead(amount);

        protected override void SkipAhead(int amount)
        {
            // If there are still items in "cache"
            if (index < CacheSize)
            {
                int delta = CacheSize - index;

                if (CanReset)
                {
                    Reset();
                }
                else
                {
                    index = CacheSize;
                }

                Position += delta;

                amount -= delta;
            }

            for (int i = 0; i < amount; i++)
            {
                MoveNext();

                if (IsEnd)
                    break;
            }
        }

        protected virtual void Reset()
        {
            index = 0;

            cache.Clear();
        }
    }
}