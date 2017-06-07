using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class SpeculativeScanner<T> : SpeculativeScannerBase<T, object>
    {
        public SpeculativeScanner(IEnumerator<T> enumerator, Func<T, T> generateEndItem = null)
            : base(enumerator, generateEndItem) { }

        public event Action<ISpeculativeScanner<T>> Speculating;

        public event Action<ISpeculativeScanner<T>, int, int> RolledBack;


        protected override object OnSpeculating()
        {
            if (Speculating != null)
                Speculating(this);

            return null;
        }

        protected override void OnRolledback(int returningFromPosition, int speculationPosition, object speculationState)
        {
            if (RolledBack != null)
                RolledBack(this, returningFromPosition, speculationPosition);
        }

    }

    public class SpeculativeScannerWithState<T, S> : SpeculativeScannerBase<T, S>
    {
        public SpeculativeScannerWithState(IEnumerator<T> enumerator, Func<T, T> generateEndItem = null)
            : base(enumerator, generateEndItem) { }

        public event Func<ISpeculativeScanner<T>, S> Speculating;

        public event Action<ISpeculativeScanner<T>, int, int, S> RolledBack;

        protected override S OnSpeculating()
        {
            return (Speculating != null) ? Speculating(this) : default(S);
        }

        protected override void OnRolledback(int returningFromPosition, int speculationPosition, S speculationState)
        {
            if (RolledBack != null)
                RolledBack(this, returningFromPosition, speculationPosition, speculationState);
        }
    }

    public abstract class SpeculativeScannerBase<T, S> : VariableLookaheadScanner<T>, ISpeculativeScanner<T>
    {
        protected struct Mark
        {
            public int Position { get; }

            public int Index { get; }

            public S State { get; }

            public Mark(int position, int index, S state)
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

        Stack<Mark> marks = new Stack<Mark>();

        public SpeculativeScannerBase(IEnumerator<T> enumerator, Func<T, T> generateEndItem = null)
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

            var store = OnSpeculating();

            marks.Push(new Mark(Position, Index, store));
        }

        protected abstract S OnSpeculating();

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

                OnRolledback(oldPosition, this.Position, mark.State);
            }
        }

        public void RollbackAll()
        {
            Rollback(SpeculationCount);
        }

        protected abstract void OnRolledback(int returningFromPosition, int speculationPosition, S speculationState);

    }
}