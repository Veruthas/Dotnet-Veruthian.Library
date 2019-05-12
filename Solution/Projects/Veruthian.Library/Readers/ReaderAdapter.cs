using System;
using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public abstract class ReaderAdapterBase<T> : IReader<T>
    {
        protected abstract IReader<T> Reader { get; }

        public virtual bool IsEnd => Reader.IsEnd;

        public virtual int Position => Reader.Position;

        public virtual void Dispose() => Reader.Dispose();

        public virtual T Current => Reader.Current;


        public virtual void Advance()
        {
            var item = Reader.Current;

            OnAdvance(item);            
        }

        protected virtual void OnAdvance(T item) { }


        public virtual void Advance(int amount)
        {
            int position = Reader.Position;

            Advance(amount);

            int actualAmount = Reader.Position - position;

            OnAdvance(amount, actualAmount);
        }

        protected virtual void OnAdvance(int requestedAmount, int actualAmount) { }
    }

    public class ReaderAdapater<T> : ReaderAdapterBase<T>
    {
        protected ReaderAdapater(IReader<T> reader)
        {
            this.Reader = reader;
        }

        protected override IReader<T> Reader { get; }
    }
}