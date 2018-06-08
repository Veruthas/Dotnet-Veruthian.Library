using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public abstract class LookaheadReaderBase<T> : ReaderBase<T>, ILookaheadReader<T>
    {
        public LookaheadReaderBase() { }

        public T Peek(int lookahead) => base.CheckedPeek(lookahead);

        public virtual IEnumerable<T> Peek(int lookahead, int amount, bool includeEnd = false)
        {
            EnsureLookahead(lookahead + amount);

            for (int i = lookahead; i < lookahead + amount; i++)
            {
                if (!PeekIsEnd(i) || includeEnd)
                    yield return Peek(i);
            }
        }

        public bool PeekIsEnd(int lookahead) => base.CheckedIsAtEnd(lookahead);
    }
}