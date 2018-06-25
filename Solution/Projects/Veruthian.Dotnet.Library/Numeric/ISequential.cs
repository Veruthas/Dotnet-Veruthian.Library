using System;

namespace Veruthian.Dotnet.Library.Numeric
{
    public interface ISequential<T> : IOrderable<T>
    {
        T Next { get; }

        T Previous { get; }        
    }
}