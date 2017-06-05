using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class EnumeratorLookaheadScanner<T> : BaseScanner<T, EnumeratorLookaheadScanner<T>>, ILookaheadScanner<T>
    {
        IEnumerator<T> items;

        T[] buffer;

        int index = 0;

        int endPosition = -1;

        T lastValid;

        Func<T, T> generateEndItem;


        public EnumeratorLookaheadScanner(IEnumerator<T> items, int lookahead, Func<T, T> generateEndItem = null)
        {
            this.items = items;

            if (lookahead < 1)
                throw new ArgumentOutOfRangeException("lookahead", string.Format("Lookahead ({0}) must be greater than 0.", lookahead));

            buffer = new T[lookahead];

            this.generateEndItem = generateEndItem ?? ((last) => default(T));
        }


        public virtual int Size { get => buffer.Length; }

        public override void Dispose() => items.Dispose();


        public override bool IsEnd => PeekIsEnd(0);

        public bool PeekIsEnd(int lookahead)
        {
            VerifyInRange(lookahead);

            VerifyInitialized();

            return Position + lookahead == endPosition;
        }

        public T Peek(int lookahead)
        {
            VerifyInitialized();

            VerifyInRange(lookahead);

            return Get(lookahead);
        }

        private void VerifyInRange(int lookahead)
        {
            if (lookahead < 0 || lookahead >= Size)
                throw new ArgumentOutOfRangeException("lookahead",
                        string.Format("Lookahead ({0}) must be a value in the range 0 to {1}.", lookahead, Size - 1));
        }

        protected override T Get(int lookahead = 0)
        {
            var wrappedIndex = (index + lookahead) % Size;

            var result = buffer[wrappedIndex];

            return result;
        }

        protected override void Initialize()
        {
            bool done = false;

            T end = default(T);

            for (int i = 0; i < Size; i++)
            {
                if (!done)
                {
                    bool success = items.MoveNext();

                    if (success)
                    {
                        lastValid = buffer[i] = items.Current;
                    }
                    else
                    {
                        done = true;

                        endPosition = i;
                        
                        end = buffer[i] = generateEndItem(lastValid);
                    }
                }
                else
                {
                    buffer[i] = end;
                }
            }
        }

        protected override void MoveToNext()
        {
            T next;

            if (endPosition == -1)
            {
                bool success = items.MoveNext();

                if (success)
                {
                    lastValid = next = items.Current;
                }
                else
                {
                    next = generateEndItem(lastValid);

                    endPosition = Position + Size;
                }
            }
            else
            {
                next = generateEndItem(lastValid);
            }

            buffer[index] = next;

            index = (index + 1) % Size;
        }
    }
}