using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class ScannerBase<T, S> : IScanner<T>
        where S : ScannerBase<T, S>
    {
        bool initialized;

        int position = -1;

        int endPosition = -1;

        T lastItem;


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


        public int Position { get => position; protected set => position = value; }


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

            if (!IsEnd)
            {
                MoveToNext();

                Position++;

                OnItemRead(current);
            }

            return current;
        }

        public void Read(int amount)
        {
            for (int i = 0; i < amount; i++)
                Read();
        }

        protected void OnItemRead(T current)
        {
            if (ItemRead != null)
                ItemRead((S)this, current);
        }

        public event Action<S, T> ItemRead;


        protected bool GetNextFromEnumerator(IEnumerator<T> enumerator, Func<T, T> generateEndItem, out T next)
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

        protected abstract void MoveToNext();

        protected abstract bool GetNext(out T next);

        protected abstract T RawPeek(int lookahead = 0);

        protected abstract void VerifyLookahead(int lookahead = 0);

        public abstract void Dispose();
    }
}