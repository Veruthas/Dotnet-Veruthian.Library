using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Readers
{
    public abstract class SpeculativeReaderBase<T> : BaseVariableLookaheadReader<T>, ISpeculativeReader<T>
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

        Stack<MarkItem> marks = new Stack<MarkItem>();


        public SpeculativeReaderBase() { }

        protected override void Initialize()
        {
            marks.Clear();

            base.Initialize();
        }

        public T PeekFromMark(int lookahead)
        {
            if (lookahead < 0)
                throw new ArgumentOutOfRangeException("lookahead", lookahead, "Lookahead cannot be less than 0");

            var mark = marks.Peek();

            int index = mark.Index + lookahead;

            EnsureIndex(index);

            var item = RawPeekByIndex(index);

            return item;
        }

        public IEnumerable<T> PeekFromMark(int lookahead, int amount, bool includeEnd = false)
        {
            if (lookahead < 0)
                throw new ArgumentOutOfRangeException("lookahead", lookahead, "Lookahead cannot be less than 0");

            if (amount < 0)
                throw new ArgumentOutOfRangeException("amount", amount, "Amount cannot be less than 0");

            var mark = marks.Peek();

            int index = mark.Index + lookahead;

            EnsureIndex(index + amount);

            for (int i = index; i < index + amount; i++)
            {
                if (i < Size || includeEnd)
                    yield return RawPeekByIndex(i);
                else
                    yield break;
            }

        }


        // Mark information
        protected Stack<MarkItem> Marks => marks;

        protected override bool CanReset => !IsSpeculating;

        public bool IsSpeculating => marks.Count != 0;


        public int MarkCount => marks.Count;

        public int MarkPosition => marks.Peek().Position;


        // Mark
        public void Mark() => CreateMark(Position, Index);

        protected void CreateMark(int position, int index) => marks.Push(new MarkItem(position, index));


        // Commit
        public void Commit()
        {
            if (!IsSpeculating)
                throw new InvalidOperationException("Cannot commit when not speculating.");

            marks.Pop();
        }


        // Rollback
        public void Rollback()
        {
            if (!IsSpeculating)
                throw new InvalidOperationException("Cannot rollback when not speculating.");

            var mark = marks.Pop();

            this.Index = mark.Index;

            this.Position = mark.Position;
        }
    }
}