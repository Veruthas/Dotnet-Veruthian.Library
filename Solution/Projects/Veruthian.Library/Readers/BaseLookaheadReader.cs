using System;
using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public abstract class BaseLookaheadReader<T> : BaseReader<T>, ILookaheadReader<T>
    {
        public BaseLookaheadReader() { }

        public T Lookahead(int amount) => base.CheckedLookahead(amount);

        public virtual IEnumerable<T> Lookahead(int amount, int length, bool includeEnd = false)
        {
            EnsureLookahead(amount + length);

            for (int i = amount; i < amount + length; i++)
            {
                if (!IsEndAhead(i) || includeEnd)
                    yield return RawLookahead(i);
            }
        }

        public bool IsEndAhead(int lookahead) => base.CheckedIsAtEnd(lookahead);
    }
}