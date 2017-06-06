using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class EnumeratorFixedLookaheadScanner<T> : BaseFixedLookaheadScanner<T, EnumeratorFixedLookaheadScanner<T>>
    {
        IEnumerator<T> enumerator;

        Func<T, T> getEndItem;

        public EnumeratorFixedLookaheadScanner(IEnumerator<T> enumerator, int lookahead, Func<T, T> getEndItem = null)
            : base(lookahead)
        {
            this.enumerator = enumerator;

            this.getEndItem = getEndItem;
        }

        public override void Dispose() => enumerator.Dispose();
        protected override bool GetNext(out T next)
        {
            if (EndPosition == -1)
            {
                bool success = enumerator.MoveNext();

                if (success)
                    next = enumerator.Current;
                else
                    next = (getEndItem != null) ? getEndItem(RawPeek(Size - 1)) : default(T);

                return success;
            }
            else
            {
                next = EndItem;

                return false;
            }
        }
    }
}