using System.Collections;
using System.Collections.Generic;
using Veruthian.Dotnet.Library.Collections.Extensions;

namespace Veruthian.Dotnet.Library.Collections
{
    public class DataPool<K, A> : IContainer<(K, A)>
    {
        Dictionary<K, (K, A)> items = new Dictionary<K, (K, A)>();

        public int Count => items.Count;

        bool IContainer<(K, A)>.Contains((K, A) value) => items.ContainsValue(value);

        public IEnumerator<(K, A)> GetEnumerator() => items.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => items.Values.GetEnumerator();

        public bool HasKey(K key) => items.ContainsKey(key);

        public (K, A) Resolve(K key, A ifUndefined)
        {
            return default((K, A));
        }

        public override string ToString() => CollectionUtility.ToTableString(this);
    }
}