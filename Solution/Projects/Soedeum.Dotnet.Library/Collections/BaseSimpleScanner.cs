using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class BaseSimpleScanner<T> : BaseScanner<T>
    {
        T current;

        int position;


        public override int Position => position;

        public override bool IsEnd => GetIsEnd(Peek());


        protected override void Initialize()
        {
            current = FetchInitial();

            position = 0;
        }

        protected override void MoveToNext(T previous)
        {
            current = FetchNext(previous);

            position++;
        }

        protected override T Get(int lookahead = 0)
        {
            return current;
        }
    }
}