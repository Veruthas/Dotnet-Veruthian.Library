using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class FixedLookaheadReader<T> : LookaheadReaderBase<T>
    {
        T[] buffer;

        int index;


        public FixedLookaheadReader(IEnumerator<T> enumerator, int lookahead, GenerateEndItem<T> generateEndItem = null)
        {
            if (lookahead < 1)
                throw new ArgumentOutOfRangeException("lookahead", string.Format("Lookahead ({0}) must be greater than 1."));

            buffer = new T[lookahead];

            SetData(enumerator, generateEndItem);
        }


        protected int Size { get => buffer.Length; }


        protected override void Initialize()
        {
            index = 0;

            bool atEnd = false;

            for (int i = 0; i < Size; i++)
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

                buffer[i] = next;
            }
        }

        protected override void EnsureLookahead(int lookahead = 0)
        {
            if (lookahead < 0 || lookahead >= Size)
                throw new ArgumentOutOfRangeException("lookahead", string.Format("Lookahead ({0}) must be in the range [1, {1}]", lookahead, Size - 1));
        }

        protected override T RawPeek(int lookahead = 0)
        {
            var actualIndex = (index + lookahead) % Size;

            var item = buffer[actualIndex];

            return item;
        }

        protected override bool MoveNext()
        {
            bool success = GetNext(out T next);

            buffer[index] = next;

            index = (index + 1) % Size;

            if (!success && !EndFound)
                EndPosition = Position + Size;

            return success;
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