using System;

namespace Veruthian.Dotnet.Library
{
    public interface IOrderable<T> : IEquatable<T>, IComparable<T>
    {
        bool Precedes(T other);

        bool Follows(T other);
    }
}