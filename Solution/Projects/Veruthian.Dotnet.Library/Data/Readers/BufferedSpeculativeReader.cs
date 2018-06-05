namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class BufferedSpeculativeReader<T> : BufferedSpeculativeReaderBase<T>
    {
        BufferedSpeculativeReader(ISpeculativeReader<T> reader, IBuffer<T> buffer)
        {
            this.SpeculativeReader = reader;

            this.Buffer = buffer;
        }
        
        protected override ISpeculativeReader<T> SpeculativeReader { get; }

        protected override ILookaheadReader<T> LookaheadReader => SpeculativeReader;

        protected override IReader<T> Reader => SpeculativeReader;

        protected override IBuffer<T> Buffer { get; }
    }
}