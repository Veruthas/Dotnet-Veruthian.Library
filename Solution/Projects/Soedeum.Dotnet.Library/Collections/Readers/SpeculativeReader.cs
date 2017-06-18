using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class SpeculativeReader<T, TState> : VariableLookaheadReader<T>, ISpeculativeReader<T, TState>
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
                return string.Format("Position: {0}; Index: {1}; State: {2}", Position, Index, ((State.Equals(null)) ? "<Null>" : State.ToString()));
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

        public TState GetMarkState(int mark) => marks[mark].State;

        public void SetMarkState(int mark, TState state)
        {
            MarkItem item = marks[mark];

            item.State = state;

            marks[mark] = item;
        }


        // Mark
        public void Mark(TState withState = default(TState))
        {
            VerifyInitialized();

            marks.Add(new MarkItem(Position, Index, withState));

            OnMarked(withState);
        }

        protected void OnMarked(TState withState)
        {
            if (Marked != null)
                Marked(this, withState);
        }

        public event SpeculationStarted<T, TState> Marked;


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

        public event SpeculationIncident<T, TState> Committed;


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
                OnRetreated(oldPosition, this.Position, mark.State);

                // Get size of retreat
                int length = (oldPosition - this.Position) + 1;

                return length;
            }

            return 0;
        }

        protected virtual void OnRetreated(int fromPosition, int originalPosition, TState originalState)
        {
            if (Retreated != null)
                Retreated(this, fromPosition, originalPosition, originalState);
        }

        public event SpeculationRetreated<T, TState> Retreated;
    }

    public class SpeculativeReader<T> : SpeculativeReader<T, Object>, ISpeculativeReader<T>
    {
        public SpeculativeReader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
            : base(enumerator, generateEndItem)
        {
        }
    }
}