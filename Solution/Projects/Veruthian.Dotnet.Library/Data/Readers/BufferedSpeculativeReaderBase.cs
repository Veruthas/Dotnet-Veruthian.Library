namespace Veruthian.Dotnet.Library.Data.Readers
{
    public abstract class BufferedSpeculativeReaderBase<T> : BufferedLookaheadReaderBase<T>, ISpeculativeReader<T>
    {
        protected abstract ISpeculativeReader<T> SpeculativeReader { get; }


        public bool IsSpeculating => SpeculativeReader.IsSpeculating;

        public int MarkCount => SpeculativeReader.MarkCount;

        public void Commit() => SpeculativeReader.Commit();

        public void Commit(int marks) => SpeculativeReader.Commit(marks);

        public void CommitAll() => SpeculativeReader.CommitAll();

        public int GetMarkPosition(int mark) => SpeculativeReader.GetMarkPosition(mark);


        public void Mark() => SpeculativeReader.Mark();

        public void Rollback()
        {            
        }

        public void Rollback(int marks)
        {            
        }

        public void RollbackAll()
        {            
        }
    }
}