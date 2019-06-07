using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public interface ILookaheadReader<out T> : IReader<T>
    {
        T Lookahead(int amount);

        IEnumerable<T> Lookahead(int amount, int length, bool includeEnd = false);
        
        bool IsEndAhead(int amount);
    }
}