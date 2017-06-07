using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class FixedLookaheadScannerBase<T, S> : LookaheadScannerBase<T, S>, ILookaheadScanner<T>
        where S : ScannerBase<T, S>
    {
        T[] buffer;

        int index = 0;




        protected FixedLookaheadScannerBase(int lookahead)
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
                    next = base.EndItem;
                }
                else
                {
                    bool success = GetNext(out next);

                    if (success)
                    {
                        LastValid = next;
                    }
                    else
                    {
                        SetEnd(i, next);

                        atEnd = true;
                    }
                }

                buffer[i] = next;
            }
        }

        protected override void MoveToNext()
        {
            bool success = GetNext(out T next);

            if (success)
                LastValid = next;
            else if (!EndFound)
                SetEnd(Position + Size , next);

            buffer[index] = next;

            index = (index + 1) % Size;
        }
    }
}