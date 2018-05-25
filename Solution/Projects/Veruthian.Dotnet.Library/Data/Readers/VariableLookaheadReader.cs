using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class VariableLookaheadReader<T> : LookaheadReaderBase<T>
    {
        List<T> buffer = new List<T>();

        int index;


        public VariableLookaheadReader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
            : base(enumerator, generateEndItem) { }


        protected int Index { get => index; set => index = value; }

        protected int Size { get => buffer.Count; }

        protected virtual bool CanReset { get => true; }



        // Prefetch(0) should do initialization
        protected override void Initialize() { }

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

        protected override bool MoveNext()
        {
            if (IsEnd)
                return false;

            index++;

            if (index == Size && CanReset)
            {
                index = 0;

                buffer.Clear();
            }

            Prefetch(1);

            return true;
        }

        // TODO: Optimize
        protected override int SkipAhead(int amount)
        {
            if (amount <= 0)
                return 0;

            for (int i = 0; i < amount; i++)
            {
                if (!MoveNext())
                    return i;
            }

            return amount;
        }
    }
}