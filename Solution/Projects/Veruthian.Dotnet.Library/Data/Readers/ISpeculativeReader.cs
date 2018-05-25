using System;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public interface ISpeculativeReader<T> : ILookaheadReader<T>
    {
        bool IsSpeculating { get; }


        int MarkCount { get; }


        int GetMarkPosition(int mark);


        void Mark();


        void Commit();

        void Commit(int marks);

        void CommitAll();


        void Rollback();

        void Rollback(int marks);

        void RollbackAll();
    }

    public interface ISpeculativeReader<T, TState> : ISpeculativeReader<T>
    {
        TState GetMarkState(int mark);

        void Mark(TState withState);
    }
}