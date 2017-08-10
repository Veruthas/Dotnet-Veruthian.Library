using System;

namespace Soedeum.Dotnet.Library.Data.Readers
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


        void Retreat();

        void Retreat(int marks);

        void RetreatAll();
    }
}