using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class BaseScanner<T, S> : IScanner<T>
        where S : BaseScanner<T, S>
    {
        bool initialized;

        int position;

        int endPosition = -1;

        T endItem;


        public bool IsEnd { get => CheckedIsAtEnd(); }

        protected bool CheckedIsAtEnd(int lookahead = 0)
        {
            VerifyInitialized();

            VerifyLookahead(lookahead);

            return RawIsAtEnd(lookahead);
        }

        protected bool RawIsAtEnd(int lookahead = 0)
        {
            return (position + lookahead) >= endPosition;
        }

        protected void SetEnd(int position, T item)
        {
            endPosition = position;

            endItem = item;
        }

        protected int EndPosition { get => endPosition; }

        protected T EndItem { get => endItem; }


        public int Position { get => position; protected set => position = value; }


        protected void VerifyInitialized()
        {
            if (!initialized)
            {
                Initialize();
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

                position++;

                OnItemRead(current);
            }

            return current;
        }

        public void Read(int amount)
        {
            for (int i = 0; i < amount; i++)
                Read();
        }


        private void OnItemRead(T current)
        {
            if (ItemRead != null)
                ItemRead((S)this, current);
        }

        public event Action<S, T> ItemRead;


        // The Abstracts
        protected abstract void Initialize();

        protected abstract void MoveToNext();

        protected abstract bool GetNext(out T next);

        protected abstract T RawPeek(int lookahead = 0);

        protected abstract void VerifyLookahead(int lookahead = 0);

        public abstract void Dispose();
    }
}