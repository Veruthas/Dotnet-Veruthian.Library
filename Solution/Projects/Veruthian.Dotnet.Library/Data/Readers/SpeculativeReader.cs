using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class SpeculativeReader<T> : VariableLookaheadReaderBase<T>, ISpeculativeReader<T>
    {
        protected struct MarkItem
        {
            public int Position { get; }

            public int Index { get; }

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
        {
            SetData(enumerator, generateEndItem);
        }

        protected override void Initialize()
        {
            marks.Clear();

            base.Initialize();            
        }

        // Mark information
        protected List<MarkItem> Marks => marks; 

        protected override bool CanReset => !IsSpeculating; 

        public bool IsSpeculating => marks.Count != 0;

        public int MarkCount => marks.Count;

        public int GetMarkPosition(int mark) => marks[mark].Position;


        // Mark
        public void Mark() => CreateMark(Position, Index);

        protected void CreateMark(int position, int index) => marks.Add(new MarkItem(position, index));


        // Commit
        public void Commit() => Commit(1);

        public void CommitAll() => Commit(marks.Count);

        public void Commit(int marks)
        {
            if (marks < -1 || marks > MarkCount)
                throw new InvalidOperationException(string.Format("Attempting to commit {0} speculations; only {1} exist", marks, MarkCount));

            if (marks > 0)
            {
                // Get mark to commit
                var markIndex = this.marks.Count - marks;

                var mark = this.marks[markIndex];

                // Pop off all committed marks
                this.marks.RemoveRange(markIndex, marks);
            }
        }


        // Rollback
        public void Rollback() => Rollback(1);

        public void RollbackAll() => Rollback(MarkCount);

        public void Rollback(int marks)
        {
            if (marks < -1 || marks > MarkCount)
                throw new InvalidOperationException(string.Format("Attempting to rollback {0} speculations; only {1} exist", marks, MarkCount));

            if (marks > 0)
            {
                // Get mark to rollback to
                var markIndex = this.marks.Count - marks;

                var mark = this.marks[markIndex];


                // Pop off all marks
                this.marks.RemoveRange(markIndex, marks);


                // Set to marked positions
                var oldPosition = Position;

                this.Index = mark.Index;

                this.Position = mark.Position;
            }
        }
    }
}