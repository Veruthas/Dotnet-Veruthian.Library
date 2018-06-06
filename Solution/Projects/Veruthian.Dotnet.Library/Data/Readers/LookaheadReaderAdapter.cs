namespace Veruthian.Dotnet.Library.Data.Readers
{
    public abstract class LookaheadReaderAdapter<T> : ReaderAdapter<T>, ILookaheadReader<T>
    {
        protected abstract ILookaheadReader<T> LookaheadReader{ get; }

        protected override IReader<T> Reader => LookaheadReader;
        

        public T Peek(int lookahead) => LookaheadReader.Peek(lookahead);

        public bool PeekIsEnd(int lookahead) => LookaheadReader.PeekIsEnd(lookahead);
    }
}