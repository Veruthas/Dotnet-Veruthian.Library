using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public abstract class SpeculativeReaderBase<T, TState> : VariableLookaheadReader<T>
    {
        protected struct MarkItem
        {
            public int Position { get; set; }

            public int Index { get; set; }

            public TState State { get; set; }

            public MarkItem(int position, int index, TState state)
            {
                this.Position = position;
                this.Index = index;
                this.State = state;
            }

            public override string ToString()
            {
                return string.Format("Position: {0}; Index: {1}", Position, Index);
            }
        }

        List<MarkItem> marks = new List<MarkItem>();


        public SpeculativeReaderBase(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
            : base(enumerator, generateEndItem)
        {
        }


        // Mark information
        protected List<MarkItem> Marks { get => marks; }

        protected override bool CanReset { get => !IsSpeculating; }
        
        public bool IsSpeculating => marks.Count != 0;

        public int MarkCount => marks.Count;

        public int GetMarkPosition(int mark) => marks[mark].Position;


        // Mark
        protected void CreateMark(int position, int index, TState state)
        {
            VerifyInitialized();

            marks.Add(new MarkItem(position, index, state));

            OnMarked(position, index, state);
        }

        protected void OnMarked(int position, int index, TState state) { }


        // Commit
        public void Commit() => Commit(1);

        public void CommitAll() => Commit(marks.Count);

        public void Commit(int marks)
        {
            if (marks < -1 || marks > MarkCount)
                throw new InvalidOperationException(string.Format("Attempting to commit {0} speculations; only {1} exist", marks, MarkCount));

            if (marks > 0)
            {
                // Get mark to commit from
                var markIndex = this.marks.Count - marks;

                var mark = this.marks[markIndex];


                // Pop off all marks
                this.marks.RemoveRange(markIndex, marks);

                // Set to marked positions
                var oldPosition = Position;

                // Notify suscribers of retreat
                OnCommitted(this.Position, oldPosition);
            }
        }

        protected virtual void OnCommitted(int markedPosition, int speculatedPosition) { }


        // Retreat
        public void Retreat() => Retreat(1);

        public void RetreatAll() => Retreat(MarkCount);

        public void Retreat(int marks)
        {
            if (marks < -1 || marks > MarkCount)
                throw new InvalidOperationException(string.Format("Attempting to rollback {0} speculations; only {1} exist", marks, MarkCount));

            if (marks > 0)
            {
                // Get mark to retreat to
                var markIndex = this.marks.Count - marks;

                var mark = this.marks[markIndex];


                // Pop off all marks
                this.marks.RemoveRange(markIndex, marks);


                // Set to marked positions
                var oldPosition = Position;

                this.Index = mark.Index;

                this.Position = mark.Position;


                // Notify retreat
                OnRetreated(this.Position, oldPosition);
            }
        }

        protected virtual void OnRetreated(int markedPosition, int speculatedPosition)
        {
        }
    }
}