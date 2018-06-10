using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public interface ISpeculativeReader<T> : ILookaheadReader<T>
    {
        bool IsSpeculating { get; }


        int MarkPosition { get; }


        T PeekFromMark(int lookahead);

        IEnumerable<T> PeekFromMark(int lookahead, int amount, bool includeEnd = false);


        void Mark();


        void Commit();


        void Rollback();
    }
}