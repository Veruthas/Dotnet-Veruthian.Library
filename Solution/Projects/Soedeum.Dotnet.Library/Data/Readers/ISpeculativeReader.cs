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

        // Returns the position retreated from
        int Retreat();

        int Retreat(int marks);

        int RetreatAll();


        event SpeculationStarted<T> Marked;

        event SpeculationCompleted<T> Committed;

        event SpeculationCompleted<T> Retreated;
    }

    
    public delegate void SpeculationStarted<T>(ISpeculativeReader<T> reader, int markedPosition);

    public delegate void SpeculationCompleted<T>(ISpeculativeReader<T> reader,
            int markedPosition, int speculatedPosition);
}