using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public interface ILookup<A, T> : IContainer<T>
    {        
        T this[A address] { get; }

        IEnumerable<A> Addresses { get; }

        IEnumerable<(A Address, T Value)> Pairs { get; }

        bool IsValidAddress(A address);

        bool TryGet(A Address, out T Value);
    }
}