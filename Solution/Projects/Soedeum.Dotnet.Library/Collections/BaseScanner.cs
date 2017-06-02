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


        public virtual T Peek()
        {
            if (!initialized)
            {
                Initialize();
                initialized = true;
            }

            var current = Get();

            return current;
        }

        public virtual T Consume()
        {
            if (!initialized)
            {
                Initialize();
                initialized = true;
            }

            var current = Get();

            if (!IsEnd)
            {
                MoveToNext();
            }

            OnItemConsumed(current);

            return current;
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