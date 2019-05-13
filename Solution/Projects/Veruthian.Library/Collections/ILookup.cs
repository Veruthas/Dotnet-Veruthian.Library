using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public interface ILookup<A, V> : IContainer<V>
    {        
        V this[A address] { get; }

        IEnumerable<A> Addresses { get; }

        IEnumerable<(A, V)> Pairs { get; }

        bool HasAddress(A address);

        bool TryGet(A address, out V value);
    }
}