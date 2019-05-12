using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public abstract class LookaheadReaderAdapterBase<T> : ReaderAdapterBase<T>, ILookaheadReader<T>
    {
        protected abstract ILookaheadReader<T> LookaheadReader{ get; }

        protected override IReader<T> Reader => LookaheadReader;
        

        public virtual T Lookahead(int amount) => LookaheadReader.Lookahead(amount);

        public virtual IEnumerable<T> Lookahead(int amount, int length, bool includeEnd = false) => LookaheadReader.Lookahead(amount, length, includeEnd);

        public virtual bool IsEndAhead(int amount) => LookaheadReader.IsEndAhead(amount);
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