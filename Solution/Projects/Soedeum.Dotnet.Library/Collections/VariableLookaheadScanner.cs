using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class VariableLookaheadScanner<T> : VariableLookaheadScannerBase<T, VariableLookaheadScanner<T>>
    {
        IEnumerator<T> enumerator;

        Func<T, T> getEndItem;

        public VariableLookaheadScanner(IEnumerator<T> enumerator, Func<T, T> getEndItem = null)
        {
            this.enumerator = enumerator;

            this.getEndItem = getEndItem;
        }

        protected override bool CanRollback => true;

        public override void Dispose() => enumerator.Dispose();

        protected override bool GetNext(out T next)
        {
            if (!EndFound)
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