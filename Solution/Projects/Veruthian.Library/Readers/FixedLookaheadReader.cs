using System;
using System.Collections.Generic;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Readers
{
    public class FixedLookaheadReader<T> : BaseLookaheadReader<T>
    {
        T[] cache;

        int index;


        public FixedLookaheadReader(IEnumerator<T> enumerator, int lookahead, GenerateEndItem<T> generateEndItem = null)
        {
            ExceptionHelper.VerifyAtLeast(lookahead, 1, nameof(lookahead));

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

        protected override void EnsureLookahead(int amount = 0)
            => ExceptionHelper.VerifyBetween(amount, 0, CacheSize - 1, "Lookahead amount");


        protected override T RawLookahead(int amount = 0)
        {
            var actualIndex = (index + amount) % CacheSize;

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