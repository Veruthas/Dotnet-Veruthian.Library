using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public interface ILookup<TKey, TValue> : IContainer<TValue>
    {
        TValue this[TKey key] { get; }

        IEnumerable<TKey> Keys { get; }

        IEnumerable<KeyValuePair<TKey, TValue>> Pairs { get; }

        bool HasKey(TKey key);

        bool TryGet(TKey key, out TValue value);
    }

    // Lookup<K, V>: TryGet<K, V>, Get<K,V>, Set<K,V>, Contains<K>, Insert<K, V>, Remove<K>,  Keys<K>, Values<V>, Pairs<K, V>
    // List<I,V>: TryGet<K, V>, Get<K, V>, Set<K, V>, Contains<K>, Insert<K, V>, Remove<K>, Add<V>,  Keys<K>, Values<V>, Pairs<K, V>
    // SortedList<I, V>: TryGet<K, V>, Get<K, V>, Set<K, V>, Contains<K>, Add<V>, Remove<K>,  Keys<K>, Values<V>, Pairs<K, V>
    // Array<I, V>: TryGet<K, V>, Get<K, V>, Set<K, V>, Contains<K>, Keys<K>, Values<V>, Pairs<K, V>

    // Set<T>: Contains<T>, Add<T>, Remove<T>, Values<T>
    // SortedSet<I, V>: TryGet<K, V>, Get<K, V>, Contains<V>, Add<V>, Remove<V>, Keys<K>, Values<V>, Pairs<K, V>


    // IContainer<T> => Contains<T>, Count
    // IExpandableContainer<T> => Add<T>, Remove<T>

    // ILookup<K, V> : IContainer<K> => Get<K, V>, TryGet<K, V>, Keys, Values, Pairs
    // IMutableLookup<K, V> : ILookup<K, V> => Set<K, V>
    // IExpandableLookup<K, V> : ISettableLookup<K, V> => Insert<K, V>, Remove<K>
}