using System;
using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public abstract class BaseSpeculativeReader<T> : BaseVariableLookaheadReader<T>, ISpeculativeReader<T>
    {
        protected struct Marker
        {
            public int Position { get; }

            public int Index { get; }

            public Marker(int position, int index)
            {
                this.Position = position;

                this.Index = index;
            }


            public override string ToString() => $"{nameof(Position)}: {Position}; {nameof(Index)}: {Index}";


            public static implicit operator (int Position, int Index)(Marker value) => (value.Position, value.Index);

            public static implicit operator Marker((int Position, int Index) value) => new Marker(value.Position, value.Index);

        }

        Stack<Marker> marks = new Stack<Marker>();


        public BaseSpeculativeReader() { }

        // Initialize
        protected override void Initialize()
        {
            marks.Clear();

            base.Initialize();
        }


        // Mark information
        protected Stack<Marker> Marks => marks;

        protected override bool CanReset => !IsSpeculating;

        public bool IsSpeculating => marks.Count != 0;


        public int MarkCount => marks.Count;

        public int MarkPosition => marks.Peek().Position;


        // Mark
        public void Mark() => CreateMark(Position, CacheIndex);

        protected void CreateMark(int position, int index) => marks.Push(new Marker(position, index));

        // PeekFromMark
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

        public IEnumerable<T> PeekFromMark(int lookahead, int? amount, bool includeEnd = false)
        {
            if (lookahead < 0)
                throw new ArgumentOutOfRangeException("lookahead", lookahead, "Lookahead cannot be less than 0");

            if (amount < 0)
                throw new ArgumentOutOfRangeException("amount", amount, "Amount cannot be less than 0");

            var realAmount = amount != null ? amount.Value : (Position - MarkPosition) + lookahead;

            var mark = marks.Peek();

            int index = mark.Index + lookahead;

            EnsureIndex(index + realAmount);

            for (int i = index; i < index + realAmount; i++)
            {
                if (i < CacheSize || includeEnd)
                    yield return RawPeekByIndex(i);
                else
                    yield break;
            }
        }


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

            this.CacheIndex = mark.Index;

            this.Position = mark.Position;
        }
    }
}