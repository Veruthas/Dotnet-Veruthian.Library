using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Readers
{
    public class ReaderAdapater<T> : ReaderAdapterBase<T>
    {
        protected ReaderAdapater(IReader<T> reader)
        {
            this.Reader = reader;
        }

        protected override IReader<T> Reader { get; }
    }

    public abstract class ReaderAdapterBase<T> : IReader<T>
    {
        protected abstract IReader<T> Reader { get; }

        public virtual bool IsEnd => Reader.IsEnd;

        public virtual int Position => Reader.Position;

        public virtual void Dispose() => Reader.Dispose();

        public virtual T Peek() => Reader.Peek();

        public virtual T Read()
        {
            var item = Reader.Read();

            OnItemRead(item);

            return item;
        }

        protected virtual void OnItemRead(T item) { }


        public virtual IEnumerable<T> Read(int amount, bool includeEnd = false)
        {
            var items = Reader.Read(amount, includeEnd);

            OnItemsRead(items);

            return items;
        }

        protected virtual void OnItemsRead(IEnumerable<T> items) { }

        public virtual void Skip(int amount)
        {
            int position = Reader.Position;

            Skip(amount);

            int actualAmount = Reader.Position - position;

            OnSkip(amount, actualAmount);
        }

        protected virtual void OnSkip(int requestedAmount, int actualAmount) { }
    }
}