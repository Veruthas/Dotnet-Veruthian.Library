using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Collections
{
    public class DataPool<K, A> : IPool<K, A>
    {
        Dictionary<K, (K, A)> items = new Dictionary<K, (K, A)>();


        public int Count => items.Count;

        bool IContainer<(K, A)>.Contains((K, A) value) => items.ContainsValue(value);

        public IEnumerator<(K, A)> GetEnumerator() => items.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => items.Values.GetEnumerator();

        public bool HasKey(K key) => items.ContainsKey(key);

        public (K, A) Resolve(K key, A attribute = default(A))
        {
            if (!items.TryGetValue(key, out var pair))
            {
                pair = (key, attribute);

                items.Add(key, pair);             
            }

            return pair;
        }

        public override string ToString() => CollectionUtility.ToTableString(this);
    }

    public class DataPool<K> : DataPool<K, object>
    {

    }
}