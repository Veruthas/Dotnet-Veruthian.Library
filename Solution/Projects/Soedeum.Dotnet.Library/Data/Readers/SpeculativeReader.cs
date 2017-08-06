using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Data.Readers
{
    public class SpeculativeReader<T> : VariableLookaheadReader<T>, ISpeculativeReader<T>
    {
        protected struct MarkItem
        {
            public int Position { get; set; }

            public int Index { get; set; }

            public MarkItem(int position, int index)
            {
                this.Position = position;
                this.Index = index;
            }

            public override string ToString()
            {
                return string.Format("Position: {0}; Index: {1}", Position, Index);
            }
        }

        List<MarkItem> marks = new List<MarkItem>();


        public SpeculativeReader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
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
        public void Mark()
        {
            VerifyInitialized();

            marks.Add(new MarkItem(Position, Index));

            OnMarked(Position);
        }

        protected void OnMarked(int position)
        {
            if (Marked != null)
                Marked(this, position);
        }

        public event SpeculationStarted<T> Marked;


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

        protected virtual void OnCommitted(int markedPosition, int speculatedPosition)
        {
            if (Committed != null)
                Committed(this, markedPosition, speculatedPosition);
        }

        public event SpeculationCompleted<T> Committed;


        // Retreat
        public int Retreat() => Retreat(1);

        public int RetreatAll() => Retreat(MarkCount);

        public int Retreat(int marks)
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


                // Notify suscribers of retreat
                OnRetreated(this.Position, oldPosition);

                // Get size of retreat
                int length = (oldPosition - this.Position);

                return length;
            }

            return 0;
        }

        protected virtual void OnRetreated(int markedPosition, int speculatedPosition)
        {
            if (Retreated != null)
                Retreated(this, speculatedPosition, markedPosition);
        }

        public event SpeculationCompleted<T> Retreated;
    }
}