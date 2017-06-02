using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class BaseScanner<T> : IScanner<T>
    {
        bool initialized;


        public abstract bool IsEnd { get; }

        public abstract int Position { get; }



        protected abstract void Initialize();

        protected abstract void MoveToNext(T previous);

        protected abstract T FetchInitial();

        protected abstract T FetchNext(T previous);

        protected abstract bool GetIsEnd(T current);

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


        public virtual T Consume()
        {
            VerifyInitialized();

            var current = Get();

            if (!IsEnd)
            {
                MoveToNext(current);
            }

            OnItemConsumed(current);

            return current;
        }

        public void Consume(int amount)
        {
            for (int i = 0; i < amount; i++)
                Consume();
        }


        public virtual void Dispose() { }

        public event Action<IScanner<T>, T> ItemConsumed;

        protected virtual void OnItemConsumed(T item)
        {
            if (ItemConsumed != null)
                ItemConsumed(this, item);
        }
    }
}