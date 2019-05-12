using System;
using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public abstract class SpeculativeReaderAdapaterBase<T> : LookaheadReaderAdapterBase<T>, ISpeculativeReader<T>
    {
        protected abstract ISpeculativeReader<T> SpeculativeReader { get; }

        protected override ILookaheadReader<T> LookaheadReader => SpeculativeReader;

        protected override IReader<T> Reader => SpeculativeReader;


        public virtual bool IsSpeculating => SpeculativeReader.IsSpeculating;


        public virtual int MarkPosition => SpeculativeReader.MarkPosition;

        public virtual T LookFromMark(int amount) => SpeculativeReader.LookFromMark(amount);

        public virtual IEnumerable<T> LookFromMark(int amount, int? length, bool includeEnd = false) => SpeculativeReader.LookFromMark(amount, length, includeEnd);

        public virtual void Mark()
        {
            int position = SpeculativeReader.Position;

            SpeculativeReader.Mark();

            OnMarked(position);
        }

        protected virtual void OnMarked(int position) { }


        public virtual void Commit()
        {

            int markedPosition = MarkPosition;

            int committedPosition = Position;

            SpeculativeReader.Commit();

            OnCommit(markedPosition, committedPosition);
        }

        protected virtual void OnCommit(int markedPosition, int committedPosition) { }


        public virtual void Rollback()
        {
            int markedPosition = MarkPosition;

            int speculatedPosition = Position;

            SpeculativeReader.Rollback();

            OnCommit(markedPosition, speculatedPosition);
        }


        protected virtual void OnRollback(int markedPosition, int speculatedPosition) { }
    }

    public class SpeculativeReaderAdapater<T> : SpeculativeReaderAdapaterBase<T>
    {
        protected SpeculativeReaderAdapater(ISpeculativeReader<T> reader)
        {
            this.SpeculativeReader = reader;
        }

        protected override ISpeculativeReader<T> SpeculativeReader { get; }
    }
}