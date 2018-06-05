namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class BufferedReader<T> : BufferedReaderBase<T>
    {
        public BufferedReader(IReader<T> reader, IBuffer<T> buffer)
        {
            this.Reader = reader;
            
            this.Buffer = buffer;
        }

        protected override IReader<T> Reader { get; }

        protected override IBuffer<T> Buffer { get; }
    }
}