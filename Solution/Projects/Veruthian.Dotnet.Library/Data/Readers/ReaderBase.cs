using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public abstract class ReaderBase<T> : IReader<T>
    {
        IEnumerator<T> enumerator;

        GenerateEndItem<T> generateEndItem;

        bool initialized;

        int position = -1;

        int endPosition = -1;

        T lastItem;


        public ReaderBase(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
        {
            this.enumerator = enumerator;

            this.generateEndItem = generateEndItem;
        }


        public int Position { get => position; protected set => position = value; }


        public virtual void Dispose() => enumerator.Dispose();


        // End
        public bool IsEnd { get => CheckedIsAtEnd(); }

        protected bool CheckedIsAtEnd(int lookahead = 0)
        {
            VerifyInitialized();

            VerifyLookahead(lookahead);

            return RawIsAtEnd(lookahead);
        }

        protected bool RawIsAtEnd(int lookahead = 0)
        {
            return (endPosition != -1) && (position + lookahead) >= endPosition;
        }

        protected bool EndFound { get => endPosition != -1; }

        protected int EndPosition { get => endPosition; set => endPosition = value; }

        protected T LastItem { get => lastItem; set => lastItem = value; }


        // Reading and Peeking
        protected void VerifyInitialized()
        {
            if (!initialized)
            {
                Initialize();

                position++;

                initialized = true;
            }
        }

        public T Peek() => CheckedPeek();

        protected T CheckedPeek(int lookahead = 0)
        {
            VerifyInitialized();

            VerifyLookahead(lookahead);

            var current = RawPeek(lookahead);

            return current;
        }

        public T Read()
        {
            VerifyInitialized();

            var current = RawPeek();

            if (MoveToNext())
            {
                Position++;

                OnItemRead(current);
            }

            return current;
        }


        protected void OnItemRead(T current) { }


        public int Skip(int amount)
        {
            var actualAmount = SkipAhead(amount);

            if (actualAmount != 0)
            {
                Position += actualAmount;

                OnItemsSkipped(amount);
            }

            return actualAmount;
        }

        protected void OnItemsSkipped(int amount) { }


        protected bool GetNext(out T next)
        {
            if (!EndFound)
            {
                bool success = enumerator.MoveNext();

                if (success)
                    next = enumerator.Current;
                else
                    next = (generateEndItem != null) ? generateEndItem(LastItem) : default(T);

                LastItem = next;

                return success;
            }
            else
            {
                next = LastItem;

                return false;
            }
        }


        // The Abstracts
        protected abstract void Initialize();

        protected abstract bool MoveToNext();

        protected abstract int SkipAhead(int amount);

        protected abstract T RawPeek(int lookahead = 0);

        protected abstract void VerifyLookahead(int lookahead = 0);
    }


    public delegate T GenerateEndItem<T>(T previous);
}