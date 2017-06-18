using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public interface ISpeculativeReader<T, TState> : ILookaheadReader<T>
    {
        bool IsSpeculating { get; }


        int MarkCount { get; }


        int GetMarkPosition(int mark);


        void SetMarkState(int mark, TState state);

        TState GetMarkState(int mark);



        void Mark(TState withState = default(TState));

        void Commit();

        int Retreat();

        int Retreat(int marks);

        int RetreatAll();


        event SpeculationStarted<T, TState> Marked;

        event SpeculationIncident<T, TState> Committed;

        event SpeculationRetreated<T, TState> Retreated;
    }

    public interface ISpeculativeReader<T> :ISpeculativeReader<T, object>
    {
        
    }

    public delegate void SpeculationStarted<T, TState>(ISpeculativeReader<T, TState> reader, TState withState);
    
    public delegate void SpeculationIncident<T, TState>(ISpeculativeReader<T, TState> reader);

    public delegate void SpeculationRetreated<T, TState>(ISpeculativeReader<T, TState> reader,
            int fromPosition, int originalPosition, TState originalState);
}