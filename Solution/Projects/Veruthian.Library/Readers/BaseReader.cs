using System;
using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public abstract class BaseReader<T> : IReader<T>
    {
        private IEnumerator<T> enumerator;

        private GenerateEndItem<T> generateEndItem;

        private int position;

        private int endPosition;

        private T lastItem;


        public BaseReader() { }


        protected void SetData(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
        {
            this.enumerator = enumerator;

            this.generateEndItem = generateEndItem;

            this.position = -1;

            this.endPosition = -1;

            this.lastItem = default(T);

            Initialize();
        }

        public virtual void Dispose() => enumerator.Dispose();



        public int Position { get => position; protected set => position = value; }



        // Fetches next item from enumerator
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


        // End
        public bool IsEnd { get => CheckedIsAtEnd(); }

        protected bool CheckedIsAtEnd(int lookahead = 0)
        {
            EnsureLookahead(lookahead);

            return RawIsAtEnd(lookahead);
        }

        protected bool RawIsAtEnd(int lookahead = 0)
        {
            return (endPosition != -1) && (position + lookahead) >= endPosition;
        }

        protected bool EndFound { get => endPosition != -1; }

        protected int EndPosition { get => endPosition; set => endPosition = value; }

        protected T LastItem { get => lastItem; set => lastItem = value; }


        // Current
        public T Current => CheckedLookahead();

        protected T CheckedLookahead(int amount = 0)
        {
            EnsureLookahead(amount);

            var current = RawLookahead(amount);

            return current;
        }

        // Advance
        public void Advance() => MoveNext();

        // Skip
        public void Advance(int amount) => SkipAhead(amount);



        // The Abstracts        
        protected abstract void Initialize();

        protected abstract void MoveNext();

        protected abstract void TryPreload(int amount);
        
        protected abstract void SkipAhead(int amount);

        protected abstract T RawLookahead(int amount = 0);

        protected abstract void EnsureLookahead(int amount = 0);
    }
}