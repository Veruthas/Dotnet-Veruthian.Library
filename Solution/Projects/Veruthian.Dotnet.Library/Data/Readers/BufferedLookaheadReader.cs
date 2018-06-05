namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class BufferedLookaheadReader<T> : BufferedLookaheadReaderBase<T>
    {
        protected BufferedLookaheadReader(ILookaheadReader<T> reader, IBuffer<T> buffer)
        {
            this.LookaheadReader = reader;

            this.Buffer = buffer;
        }

        protected override IReader<T> Reader => LookaheadReader;

        protected override ILookaheadReader<T> LookaheadReader { get; }

        protected override IBuffer<T> Buffer { get; }
    }
}