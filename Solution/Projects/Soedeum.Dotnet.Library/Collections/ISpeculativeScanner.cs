using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public interface ISpeculativeScanner<T> : ILookaheadScanner<T>
    {
        int SpeculationCount { get; }

        bool IsSpeculating { get; }

        void Speculate();

        void Commit();

        void Rollback();

        void Rollback(int speculations);

        void RollbackAll();
    }
}