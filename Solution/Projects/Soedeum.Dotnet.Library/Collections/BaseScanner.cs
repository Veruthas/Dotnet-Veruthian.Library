using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class BaseScanner<T> : IScanner<T>
    {
        bool initialized;


        public abstract bool IsEnd { get; }

        public abstract int Position { get; }


        protected abstract void Initialize();

        protected abstract void MoveToNext();

        protected abstract T Get(int lookahead = 0);


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
            }

            return current;
        }

        public void Read(int amount)
        {
            for (int i = 0; i < amount; i++)
                Read();
        }


        public virtual void Dispose() { }
    }
}