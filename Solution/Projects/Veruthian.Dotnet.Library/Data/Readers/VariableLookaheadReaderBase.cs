using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class VariableLookaheadReaderBase<T> : LookaheadReaderBase<T>
    {
        List<T> items = new List<T>();

        int index;


        public VariableLookaheadReaderBase() { }


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

        protected override void EnsureLookahead(int lookahead = 0)
        {
            // Find out if we have enough lookahead.
            int available = (Size - index);

            int difference = available - lookahead;

            // Prefetch (d + 1) items (since index and lookaheads are indices)
            if (difference <= 0)
                Prefetch(difference + 1);
        }

        private int Prefetch(int amount)
        {
            if (EndFound)
                return 0;

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

                    return i;
                }
            }

            return amount;
        }
        
        protected T RawPeekIndex(int index)
        {
            // We don't want to add infinite end items, just return the cached end item
            if (EndFound && index >= items.Count)
                return LastItem;
            else
                return items[index];
        }

        protected override T RawPeek(int lookahead = 0)
        {
            // We don't want to add infinite end items, just return the cached end item
            if (EndFound && index + lookahead >= items.Count)
                return LastItem;
            else
                return items[index + lookahead];
        }

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