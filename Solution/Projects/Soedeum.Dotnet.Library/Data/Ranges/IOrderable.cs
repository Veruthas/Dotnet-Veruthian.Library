using System;

namespace Soedeum.Dotnet.Library.Data.Ranges
{
    public interface IOrderable<T> : IEquatable<T>, IComparable<T>
    {
        T Default { get; }

        T MinValue { get; }

        T MaxValue { get; }

        T Next();

        T Previous();


        bool LessThan(T other);

        bool GreaterThan(T other);
    }
}