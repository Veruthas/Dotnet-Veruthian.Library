using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public interface ISpeculativeScanner<T> : ILookaheadScanner<T>
    {
        // The number of marks that have been pushed
        int MarkCount { get; }

        // Sets a mark for backtracking
        void Mark();

        // Commits everything until the first mark
        void Commit();

        // Rollback to previous mark
        void Rollback();

        // The position of Mark[n] in the Scanner
        int GetMarkPosition(int index);

        // The number of items read since the mark position
        int GetLengthToMark(int index);
    }
}