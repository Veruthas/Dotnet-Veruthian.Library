using System;

namespace Veruthian.Dotnet.Library.Data
{
    public interface ISequential<T> : IOrderable<T>
    {
        T Next { get; }

        T Previous { get; }
    }
}