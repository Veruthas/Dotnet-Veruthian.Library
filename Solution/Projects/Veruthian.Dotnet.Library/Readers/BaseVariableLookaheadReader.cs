using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Readers
{
    public abstract class BaseVariableLookaheadReader<T> : BaseLookaheadReader<T>
    {
        List<T> items = new List<T>();

        int index;


        public BaseVariableLookaheadReader() { }


        protected int Index { get => index; set => index = value; }

        protected List<T> Items { get => items; }

        protected int Size { get => items.Count; }

        protected virtual bool CanReset { get => true; }


        protected override void Initialize()
        {
            items.Clear();

            index = 0;

            Position = 0;
        }


        protected void EnsureIndex(int index)
        {
            int available = (Size - index);

            // Prefetch (d + 1) items
            if (available <= 0)
                Prefetch(available + 1);
        }

        protected override void EnsureLookahead(int lookahead = 0) => EnsureIndex(index + lookahead);


        private void Prefetch(int amount)
        {
            if (!EndFound)
            {
                int lastPosition = Size;

                for (int i = 0; i < amount; i++)
                {
                    bool success = GetNext(out T next);

                    if (success)
                    {
                        items.Add(next);
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
            if (EndFound && index >= items.Count)
                return LastItem;
            else
                return items[index];
        }

        protected override T RawPeek(int lookahead = 0) => RawPeekByIndex(index + lookahead);


        protected override void MoveNext()
        {
            if (!IsEnd)
            {
                Position++;

                index++;

                if (index == Size && CanReset)
                {
                    index = 0;

                    items.Clear();
                }

                EnsureLookahead(1);
            }
        }

        protected override void TryPreload(int amount) => EnsureLookahead(amount);

        protected override void SkipAhead(int amount)
        {
            if (index < Size)
            {
                int delta = Size - index;

                if (CanReset)
                {
                    index = 0;

                    items.Clear();
                }
                else
                {
                    index = Size;
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
    }
}