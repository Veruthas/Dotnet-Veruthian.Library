using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public abstract class LookaheadReaderAdapterBase<T> : ReaderAdapterBase<T>, ILookaheadReader<T>
    {
        protected abstract ILookaheadReader<T> LookaheadReader{ get; }

        protected override IReader<T> Reader => LookaheadReader;
        

        public virtual T Peek(int lookahead) => LookaheadReader.Peek(lookahead);

        public virtual IEnumerable<T> Peek(int lookahead, int amount, bool includeEnd = false) => LookaheadReader.Peek(lookahead, amount, includeEnd);

        public virtual bool PeekIsEnd(int lookahead) => LookaheadReader.PeekIsEnd(lookahead);
    }

    public class LookaheadReaderAdapter<T> : LookaheadReaderAdapterBase<T>
    {
        protected LookaheadReaderAdapter(ILookaheadReader<T> reader)
        {
            this.LookaheadReader = reader;
        }

        protected override ILookaheadReader<T> LookaheadReader { get; }
    }
}