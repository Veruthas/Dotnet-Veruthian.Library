using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Readers
{
    public delegate T GenerateEndItem<T>(T previous);


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


        // Peek
        public T Peek() => CheckedPeek();

        protected T CheckedPeek(int lookahead = 0)
        {
            EnsureLookahead(lookahead);

            var current = RawPeek(lookahead);

            return current;
        }


        // Read
        public T Read()
        {
            var current = RawPeek();

            MoveNext();

            return current;
        }

        public IEnumerable<T> Read(int amount, bool includeEnd = false)
        {
            TryPreload(amount);

            for (int i = 0; i < amount; i++)
            {
                if (!IsEnd || includeEnd)
                    yield return Read();
            }
        }

        // Skip
        public void Skip(int amount) => SkipAhead(amount);



        // The Abstracts        
        protected abstract void Initialize();

        protected abstract void MoveNext();

        protected abstract void TryPreload(int amount);
        
        protected abstract void SkipAhead(int amount);

        protected abstract T RawPeek(int lookahead = 0);

        protected abstract void EnsureLookahead(int lookahead = 0);
    }
}