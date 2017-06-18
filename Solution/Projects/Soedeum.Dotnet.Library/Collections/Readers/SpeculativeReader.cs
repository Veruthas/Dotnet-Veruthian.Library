using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
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
                return string.Format("Position: {0}; Index: {1}", Position, Index, );
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

            OnMarked();
        }

        protected void OnMarked()
        {
            if (Marked != null)
                Marked(this);
        }

        public event SpeculationIncident<T> Marked;


        // Commit
        public void Commit()
        {
            if (!IsSpeculating)
                throw new InvalidOperationException("No speculations to commit to!");

            marks.Clear();

            OnCommitted();
        }

        protected virtual void OnCommitted()
        {
            if (Committed != null)
                Committed(this);
        }

        public event SpeculationIncident<T> Committed;


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
                OnRetreated(oldPosition, this.Position);

                // Get size of retreat
                int length = (oldPosition - this.Position) + 1;

                return length;
            }

            return 0;
        }

        protected virtual void OnRetreated(int fromPosition, int originalPosition)
        {
            if (Retreated != null)
                Retreated(this, fromPosition, originalPosition);
        }

        public event SpeculationRetreated<T> Retreated;
    }
}