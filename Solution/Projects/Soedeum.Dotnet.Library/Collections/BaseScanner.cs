using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class BaseScanner<T, S> : IScanner<T>
        where S : BaseScanner<T, S>
    {
        bool initialized;

        int position;


        public abstract bool IsEnd { get; }

        public int Position { get => position; protected set => position = value; }


        protected abstract void Initialize();

        protected abstract void MoveToNext();

        protected abstract T Get(int lookahead = 0);

        public abstract void Dispose();


        protected void VerifyInitialized()
        {
            if (!initialized)
            {
                Initialize();
                initialized = true;
            }
        }

        public virtual T Peek()
        {
            VerifyInitialized();

            var current = Get();

            return current;
        }


        public virtual T Read()
        {
            VerifyInitialized();

            var current = Get();

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
    }
}