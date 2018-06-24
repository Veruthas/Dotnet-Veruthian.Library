using System;

namespace Veruthian.Dotnet.Library.Data
{
    public interface IOrderable<T> : IEquatable<T>, IComparable<T>
    {
        T Default { get; }


        T MinValue { get; }

        T MaxValue { get; }


        bool Precedes(T other);

        bool Follows(T other);
    }
}