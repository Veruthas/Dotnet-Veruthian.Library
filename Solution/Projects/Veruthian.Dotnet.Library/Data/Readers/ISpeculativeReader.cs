using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public interface ISpeculativeReader<T> : ILookaheadReader<T>
    {
        bool IsSpeculating { get; }


        int MarkCount { get; }


        int GetMarkPosition(int mark);


        T PeekFromMark(int mark, int lookahead);

        IEnumerable<T> PeekFromMark(int mark, int lookahead, int amount, bool includeEnd = false);


        void Mark();


        void Commit();

        void Commit(int marks);

        void CommitAll();


        void Rollback();

        void Rollback(int marks);

        void RollbackAll();
    }
}