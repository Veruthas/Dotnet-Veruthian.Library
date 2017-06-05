using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class EnumeratorSimpleScanner<T> : BaseSimpleScanner<T, EnumeratorSimpleScanner<T>>
    {
        IEnumerator<T> enumerator;

        Func<T, T> getEndItem;

        public EnumeratorSimpleScanner(IEnumerator<T> enumerator, Func<T, T> getEndItem = null)
        {
            this.enumerator = enumerator;

            this.getEndItem = getEndItem;
        }

        public override void Dispose() => enumerator.Dispose();

        protected T GetLastValid() => RawPeek();

        protected override bool GetNext(out T next)
        {
            bool success = enumerator.MoveNext();

            if (success)
                next = enumerator.Current;
            else
                next = (getEndItem != null) ? getEndItem(GetLastValid()) : default(T);            
                        
            return success;
        }
    }
}