using System;

namespace Veruthian.Dotnet.Library.Data
{
    public interface ISequential<T> : IEquatable<T>, IComparable<T>
    {
        T Default { get; }


        T MinValue { get; }

        T MaxValue { get; }


        T Next { get; }

        T Previous { get; }


        bool IsLessThan(T other);

        bool IsGreaterThan(T other);
    }
}