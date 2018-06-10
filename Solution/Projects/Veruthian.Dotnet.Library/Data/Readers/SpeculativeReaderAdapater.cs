using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class SpeculativeReaderAdapater<T> : SpeculativeReaderAdapaterBase<T>
    {
        protected SpeculativeReaderAdapater(ISpeculativeReader<T> reader)
        {
            this.SpeculativeReader = reader;
        }

        protected override ISpeculativeReader<T> SpeculativeReader { get; }
    }

    public abstract class SpeculativeReaderAdapaterBase<T> : LookaheadReaderAdapterBase<T>, ISpeculativeReader<T>
    {
        protected abstract ISpeculativeReader<T> SpeculativeReader { get; }

        protected override ILookaheadReader<T> LookaheadReader => SpeculativeReader;

        protected override IReader<T> Reader => SpeculativeReader;


        public virtual bool IsSpeculating => SpeculativeReader.IsSpeculating;


        public virtual int MarkCount => SpeculativeReader.MarkCount;

        public virtual int GetMarkPosition(int marksBack) => SpeculativeReader.GetMarkPosition(marksBack);

        public virtual T PeekFromMark(int marksBack, int lookahead) => SpeculativeReader.PeekFromMark(marksBack, lookahead);

        public virtual IEnumerable<T> PeekFromMark(int marksBack, int lookahead, int amount, bool includeEnd = false) 
            => SpeculativeReader.PeekFromMark(marksBack, lookahead, amount, includeEnd);


        public virtual void Mark()
        {
            int position = SpeculativeReader.Position;

            SpeculativeReader.Mark();

            OnMarked(position);
        }

        protected virtual void OnMarked(int position) { }


        public virtual void Commit()
        {
            int markCount = SpeculativeReader.MarkCount;

            int markedPosition = GetMarkPosition(markCount - 1);

            int committedPosition = Position;

            SpeculativeReader.Commit();

            OnCommit(markedPosition, committedPosition);
        }

        public virtual void Commit(int marksBack)
        {
            int markCount = SpeculativeReader.MarkCount;

            int markedPosition = GetMarkPosition(markCount - marksBack);

            int committedPosition = Position;

            SpeculativeReader.Commit(marksBack);

            OnCommit(markedPosition, committedPosition);
        }

        public virtual void CommitAll()
        {
            int markedPosition = GetMarkPosition(0);

            int committedPosition = Position;

            SpeculativeReader.CommitAll();

            OnCommit(markedPosition, committedPosition);
        }

        protected virtual void OnCommit(int markedPosition, int committedPosition) { }


        public virtual void Rollback()
        {
            int markCount = SpeculativeReader.MarkCount;

            int markedPosition = GetMarkPosition(markCount - 1);

            int speculatedPosition = Position;

            SpeculativeReader.Rollback();

            OnCommit(markedPosition, speculatedPosition);
        }

        public virtual void Rollback(int marksBack)
        {
            int markCount = SpeculativeReader.MarkCount;

            int markedPosition = GetMarkPosition(markCount - marksBack);

            int speculatedPosition = Position;

            SpeculativeReader.Rollback(marksBack);

            OnCommit(markedPosition, speculatedPosition);
        }

        public virtual void RollbackAll()
        {
            int markedPosition = GetMarkPosition(0);

            int speculatedPosition = Position;

            SpeculativeReader.RollbackAll();

            OnCommit(markedPosition, speculatedPosition);
        }


        protected virtual void OnRollback(int markedPosition, int speculatedPosition) { }
    }
}