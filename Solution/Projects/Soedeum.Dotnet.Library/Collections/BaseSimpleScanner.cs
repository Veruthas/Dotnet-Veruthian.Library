using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class BaseSimpleScanner<T, S> : BaseScanner<T, BaseSimpleScanner<T, S>>
        where S : BaseSimpleScanner<T, S>
    {
        T item;

        protected override T RawGet(int lookahead = 0) => item;

        protected override void Initialize() => ProcessMoveToNext();

        protected override void ProcessMoveToNext()
        {
            bool success = MoveToNext(out T next);

            if (!success)
                SetEnd(Position, next);

            item = next;
        }

        protected override void VerifyLookahead(int lookahead = 0) { }
    }
}