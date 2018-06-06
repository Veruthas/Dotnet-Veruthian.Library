namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class LookaheadReaderAdapter<T> : LookaheadReaderAdapterBase<T>
    {
        protected LookaheadReaderAdapter(ILookaheadReader<T> reader)
        {
            this.LookaheadReader = reader;
        }

        protected override ILookaheadReader<T> LookaheadReader { get; }
    }


    public abstract class LookaheadReaderAdapterBase<T> : ReaderAdapterBase<T>, ILookaheadReader<T>
    {
        protected abstract ILookaheadReader<T> LookaheadReader{ get; }

        protected override IReader<T> Reader => LookaheadReader;
        

        public T Peek(int lookahead) => LookaheadReader.Peek(lookahead);

        public bool PeekIsEnd(int lookahead) => LookaheadReader.PeekIsEnd(lookahead);
    }
}