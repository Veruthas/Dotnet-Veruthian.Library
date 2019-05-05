using System;
using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public class FixedLookaheadReader<T> : BaseLookaheadReader<T>
    {
        T[] cache;

        int index;


        public FixedLookaheadReader(IEnumerator<T> enumerator, int lookahead, GenerateEndItem<T> generateEndItem = null)
        {
            if (lookahead < 1)
                throw new ArgumentOutOfRangeException("lookahead", "Lookahead must be greater than 1.");

            cache = new T[lookahead];

            SetData(enumerator, generateEndItem);
        }

        protected T[] Cache => cache;
        
        protected int CacheSize => cache.Length; 


        protected override void Initialize()
        {
            Position = 0;

            PopulateLookahead();
        }

        private void PopulateLookahead()
        {
            index = 0;

            bool atEnd = IsEnd;

            for (int i = 0; i < CacheSize; i++)
            {
                T next;

                if (atEnd)
                {
                    next = LastItem;
                }
                else
                {
                    bool success = GetNext(out next);

                    if (!success)
                    {
                        EndPosition = i;

                        atEnd = true;
                    }

                    LastItem = next;
                }

                cache[i] = next;
            }
        }

        protected override void EnsureLookahead(int lookahead = 0)
        {
            if (lookahead < 0 || lookahead > CacheSize)
                throw new ArgumentOutOfRangeException("lookahead", string.Format("Lookahead must be in the range [0, {1}]", CacheSize - 1));
        }

        protected override T RawPeek(int lookahead = 0)
        {
            var actualIndex = (index + lookahead) % CacheSize;

            var item = cache[actualIndex];

            return item;
        }

        protected override void MoveNext()
        {
            bool success = GetNext(out T next);

            if (!IsEnd)
                Position++;

            if (!success && !EndFound)
                EndPosition = Position + CacheSize - 1;

            cache[index] = next;

            index = (index + 1) % CacheSize;
        }

        protected override void TryPreload(int amount) { }

        // TODO: Optimize
        protected override void SkipAhead(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                MoveNext();

                if (IsEnd)
                    break;
            }
        }
    }
}