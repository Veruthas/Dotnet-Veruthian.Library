using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class BaseLookaheadScanner<T> : BaseScanner<T>, ILookaheadScanner<T>
    {
        T[] items;

        int index;

        int position;

        int endPosition;

        public BaseLookaheadScanner(int lookahead)
        {
            if (lookahead < 1)
                throw new ArgumentOutOfRangeException("lookahead",
                    "Lookahead must be greater than 0");

            this.items = new T[lookahead];
        }

        public override bool IsEnd => position == endPosition;

        public override int Position => position;

        private int Size => items.Length;

        public T Peek(int lookahead)
        {
            if (lookahead >= Size)
                throw new ArgumentOutOfRangeException("lookahead",
                    string.Format("Scanner only supports {0} items of lookahead", Size));

            VerifyInitialized();

            var current = Get(lookahead);

            return current;
        }

        protected override T Get(int lookahead = 0)
        {
            var actual = (index + lookahead) % Size;

            return items[actual];
        }

        protected override void Initialize()
        {
            items[0] = FetchInitial();

            if (Size > 1)
                for (int i = 1; i < Size; i++)
                {
                    var next = FetchNext(items[i - 1]);
                }
        }

        protected override void MoveToNext(T previous)
        {
            items[index] = FetchNext(previous);

            index = (index + 1) % Size;

            position++;
        }

        
    }
}