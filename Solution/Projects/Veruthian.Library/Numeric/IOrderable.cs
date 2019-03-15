using System;

namespace Veruthian.Library.Numeric
{
    public interface IOrderable<T> : IEquatable<T>, IComparable<T>
    {
        bool Precedes(T other);

        bool Follows(T other);
    }
}