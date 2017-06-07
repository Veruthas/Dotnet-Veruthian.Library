using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class VariableLookaheadScannerBase<T, S> : LookaheadScannerBase<T, S>, ILookaheadScanner<T>
        where S : ScannerBase<T, S>
    {
        List<T> buffer = new List<T>();

        int index;

        int size;


        protected int Index { get => index; set => index = value; }

        protected int Size { get => size; }


        protected abstract bool CanRollback { get; }


        protected override void VerifyLookahead(int lookahead = 0)
        {
            int difference = size - index;

            if (difference <= lookahead)
            {
                Prefetch(difference);
            }
        }

        private void Prefetch(int amount)
        {
            if (EndFound)
                return;

            int lastPosition = amount;

            for (int i = 0; i < amount; i++)
            {
                bool success = GetNext(out T next);

                if (success)
                {
                    LastValid = next;

                    if (size == buffer.Count)
                        buffer.Add(next);
                    else
                        buffer[size] = next;

                    size++;
                }
                else
                {
                    SetEnd(lastPosition + i, next);
                    break;
                }
            }
        }

        protected override T RawPeek(int lookahead = 0)
        {
            // We don't want to add infinite end items, the set one should just be returned
            if (EndFound && Position + lookahead >= EndPosition)
                return EndItem;
            else
                return buffer[index + lookahead];
        }

        // Prefetch(0) should've done this for us
        protected override void Initialize() { }

        protected override void MoveToNext()
        {
            index++;

            if (index == size && CanRollback)
                index = size = 0;

            Prefetch(1);
        }
    }
}