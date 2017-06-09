using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public interface ISpeculativeReader<T, S> : ILookaheadReader<T>
    {
        int SpeculationCount { get; }

        bool IsSpeculating { get; }

        void Speculate();

        void Commit();

        void Retract();

        void Retract(int speculations);

        void RetractAll();


        event SpeculationBegun<T, S> Speculating;

        event Action<ISpeculativeReader<T, S>> Committed;

        event SpeculationRetracted<T, S> Retracted;
    }

    public interface ISpeculativeReader<T> : ISpeculativeReader<T, object>
    {

    }

    public delegate S SpeculationBegun<T, S>(ISpeculativeReader<T, S> reader);

    public delegate void SpeculationRetracted<T, S>(ISpeculativeReader<T, S> reader,
            int fromPosition, int originalPosition, S originalState);
}