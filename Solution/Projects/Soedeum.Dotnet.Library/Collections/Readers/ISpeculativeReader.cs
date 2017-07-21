using System;

namespace Soedeum.Dotnet.Library.Collections.Readers
{
    public interface ISpeculativeReader<T> : ILookaheadReader<T>
    {
        bool IsSpeculating { get; }


        int MarkCount { get; }


        int GetMarkPosition(int mark);



        void Mark();

        void Commit();


        // Returns the position retreated from
        int Retreat();

        int Retreat(int marks);

        int RetreatAll();


        event SpeculationIncident<T> Marked;

        event SpeculationIncident<T> Committed;

        event SpeculationRetreated<T> Retreated;
    }

    
    public delegate void SpeculationIncident<T>(ISpeculativeReader<T> reader);

    public delegate void SpeculationRetreated<T>(ISpeculativeReader<T> reader,
            int fromPosition, int originalPosition);
}