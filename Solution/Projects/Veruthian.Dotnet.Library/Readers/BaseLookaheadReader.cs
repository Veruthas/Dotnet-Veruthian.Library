using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Readers
{
    public abstract class BaseLookaheadReader<T> : BaseReader<T>, ILookaheadReader<T>
    {
        public BaseLookaheadReader() { }

        public T Peek(int lookahead) => base.CheckedPeek(lookahead);

        public virtual IEnumerable<T> Peek(int lookahead, int amount, bool includeEnd = false)
        {
            EnsureLookahead(lookahead + amount);

            for (int i = lookahead; i < lookahead + amount; i++)
            {
                if (!PeekIsEnd(i) || includeEnd)
                    yield return RawPeek(i);
            }
        }

        public bool PeekIsEnd(int lookahead) => base.CheckedIsAtEnd(lookahead);
    }
}