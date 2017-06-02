using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class BaseSimpleScanner<T> : BaseScanner<T>
    {
        T current;

        int position;


        public override int Position => position;


        protected override void Initialize()
        {
            current = FetchInitial();

            position = 0;
        }

        protected override void MoveToNext()
        {
            current = FetchNext(current);

            position++;
        }

        protected override T Get(int lookahead = 0)
        {
            return current;
        }

        protected abstract T FetchInitial();

        protected abstract T FetchNext(T previous);
    }
}