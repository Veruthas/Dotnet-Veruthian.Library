using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Readers
{
    public interface ILookaheadReader<out T> : IReader<T>
    {
        T Peek(int lookahead);

        IEnumerable<T> Peek(int lookahead, int amount, bool includeEnd = false);
        
        bool PeekIsEnd(int lookahead);
    }
}