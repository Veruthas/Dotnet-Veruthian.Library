using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public interface ILookup<A, V> : IContainer<V>
    {        
        V this[A address] { get; }

        IEnumerable<A> Addresses { get; }

        IEnumerable<(A Address, V Value)> Pairs { get; }

        bool HasAddress(A address);

        bool TryGet(A Address, out V Value);
    }
}