using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class SpeculativeScanner<T> : VariableLookaheadScanner<T>, ISpeculativeScanner<T>
    {
        protected struct Mark
        {
            public int Position { get; }

            public int Index { get; }


            public Mark(int position, int index)
            {
                this.Position = position;
                this.Index = index;
            }

            public override string ToString()
            {
                return string.Format("Position: {0}; Index: {1}", Position, Index);
            }
        }

        Stack<Mark> marks = new Stack<Mark>();

        public SpeculativeScanner(IEnumerator<T> enumerator, Func<T, T> generateEndItem = null)
            : base(enumerator, generateEndItem)
        {
        }


        protected Stack<Mark> Marks { get => marks; }


        protected override bool CanReset { get => !IsSpeculating; }


        public int SpeculationCount => marks.Count;

        public bool IsSpeculating => marks.Count != 0;


        public void Speculate()
        {
            VerifyInitialized();

            marks.Push(new Mark(Position, Index));

            OnSpeculating();
        }

        public event Action<ISpeculativeScanner<T>> Speculating;

        protected virtual void OnSpeculating()
        {
            if (Speculating != null)
                Speculating(this);
        }

        public void Commit()
        {
            if (!IsSpeculating)
                throw new InvalidOperationException("No speculations to commit to!");

            marks.Clear();

            OnCommitted();
        }

        public event Action<ISpeculativeScanner<T>> Committed;

        protected virtual void OnCommitted()
        {
            if (Committed != null)
                Committed(this);
        }

        public void Rollback()
        {
            Rollback(1);
        }

        public void Rollback(int speculations)
        {
            if (speculations < -1 || speculations > SpeculationCount)
                throw new InvalidOperationException(string.Format("Attempting to rollback {0} speculations; only {1} exist", speculations, SpeculationCount));

            if (speculations > 0)
            {
                Mark mark = default(Mark);

                for (int i = 0; i < speculations; i++)
                    mark = marks.Pop();


                int oldPosition = Position;

                this.Index = mark.Index;

                this.Position = mark.Position;

                OnRollback(oldPosition, this.Position);
            }
        }

        public void RollbackAll()
        {
            Rollback(SpeculationCount);
        }

        public event Action<ISpeculativeScanner<T>, int, int> Rolledback;

        protected virtual void OnRollback(int from, int to)
        {
            if (Rolledback != null)
                Rolledback(this, from, to);
        }

    }
}