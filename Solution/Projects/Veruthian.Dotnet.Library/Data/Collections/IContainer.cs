using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    /* 
        IContainer<T>
        IExpandableContainer<T>

        ILookup<K, V> : IContainer<V>
        IMutableLookup<K, V> : ILookup<K, V>
        IExpandableLookup<K, V> : IMutableLookup<K, V>

        IIndex<T> : ILookup<int, T>        
        IMutableIndex<T> : IIndex<T>, IMutableLookup<int, T>
        IOrderedIndex<T> : IIndex<T>, IExpandableContainer<T>
        IExpandableIndex<T> : IMutableIndex<T>, IExpandableLookup<int, T>, IExpandableContainer<T>
    */

    public interface IContainer<T>
    {
        int Count { get; }

        bool Contains(T value);

        IEnumerable<T> Values { get; }
    }
}