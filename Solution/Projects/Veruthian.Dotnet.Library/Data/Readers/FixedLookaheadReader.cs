using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class FixedLookaheadReader<T> : BaseLookaheadReader<T>
    {
        T[] buffer;

        int index = 0;

        public FixedLookaheadReader(IEnumerator<T> enumerator, int lookahead, GenerateEndItem<T> generateEndItem = null)
            : base(enumerator, generateEndItem)
        {
            if (lookahead < 1)
                throw new ArgumentOutOfRangeException("lookahead", string.Format("Lookahead ({0}) must be greater than 1."));

            buffer = new T[lookahead];
        }


        protected int Size { get => buffer.Length; }


        protected override void VerifyLookahead(int lookahead = 0)
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


        protected override void Initialize()
        {
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

        protected override void MoveToNext()
        {
            bool success = GetNext(out T next);

            if (!EndFound)
                EndPosition = Position + Size;

            buffer[index] = next;

            index = (index + 1) % Size;
        }
    }
}