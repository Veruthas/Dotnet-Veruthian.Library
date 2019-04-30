using System;
using System.Collections.Generic;
using Veruthian.Library.Processing;

namespace Veruthian.Library.Readers
{
    public interface ISpeculativeReader<out T> : ILookaheadReader<T>, ISpeculative
    {
        int MarkPosition { get; }

        T PeekFromMark(int lookahead);

        IEnumerable<T> PeekFromMark(int lookahead, int amount, bool includeEnd = false);
    }
}