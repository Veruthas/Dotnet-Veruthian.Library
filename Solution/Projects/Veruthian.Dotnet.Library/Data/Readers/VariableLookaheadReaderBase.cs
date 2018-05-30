using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class VariableLookaheadReaderBase<T> : LookaheadReaderBase<T>
    {
        List<T> buffer = new List<T>();

        int index;


        public VariableLookaheadReaderBase() { }


        protected int Index { get => index; set => index = value; }

        protected int Size { get => buffer.Count; }

        protected virtual bool CanReset { get => true; }


        protected override void Initialize()
        {
            buffer.Clear();

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
                    buffer.Add(next);
                }
                else
                {
                    EndPosition = lastPosition + i;

                    return i;
                }
            }

            return amount;
        }

        protected override T RawPeek(int lookahead = 0)
        {
            // We don't want to add infinite end items, just return the cached end item
            if (EndFound && Position + lookahead >= EndPosition)
                return LastItem;
            else
                return buffer[index + lookahead];
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

                    buffer.Clear();
                }

                EnsureLookahead(1);
            }
        }

        // TODO: Optimize
        protected override void SkipAhead(int amount)
        {
            if (index + amount <= Size)
            {                
                Position += amount;

                index += amount;
            }
            else
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
}