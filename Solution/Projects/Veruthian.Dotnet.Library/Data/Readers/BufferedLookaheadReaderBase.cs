namespace Veruthian.Dotnet.Library.Data.Readers
{
    public abstract class BufferedLookaheadReaderBase<T> : BufferedReaderBase<T>, ILookaheadReader<T>
    {
        protected abstract ILookaheadReader<T> LookaheadReader { get; }


        public T Peek(int lookahead) => LookaheadReader.Peek(lookahead);

        public bool PeekIsEnd(int lookahead) => LookaheadReader.PeekIsEnd(lookahead);
    }
}