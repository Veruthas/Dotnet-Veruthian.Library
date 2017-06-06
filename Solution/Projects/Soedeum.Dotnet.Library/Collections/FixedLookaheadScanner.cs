using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class FixedLookaheadScanner<T> : FixedLookaheadScannerBase<T, FixedLookaheadScanner<T>>
    {
        IEnumerator<T> enumerator;

        Func<T, T> getEndItem;

        public FixedLookaheadScanner(IEnumerator<T> enumerator, int lookahead, Func<T, T> getEndItem = null)
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
                {
                    var last = LastValid;
                    
                    next = (getEndItem != null) ? getEndItem(last) : default(T);
                }

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