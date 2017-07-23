using System;

namespace Soedeum.Dotnet.Library.Data.Ranges
{
    public interface IOrderable<T> : IEquatable<T>, IComparable<T>
    {
        T Next();

        T Previous();


        T Default { get; }

        T MinValue { get; }

        T MaxValue { get; }
    }
}