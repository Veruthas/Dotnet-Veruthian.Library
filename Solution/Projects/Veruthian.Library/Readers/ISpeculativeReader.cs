using System;
using System.Collections.Generic;
using Veruthian.Library.Processing;

namespace Veruthian.Library.Readers
{
    public interface ISpeculativeReader<out T> : ILookaheadReader<T>, ISpeculator
    {
        int MarkPosition { get; }

        T LookFromMark(int amount);

        IEnumerable<T> LookFromMark(int amount, int? length, bool includeEnd = false);        
    }
}