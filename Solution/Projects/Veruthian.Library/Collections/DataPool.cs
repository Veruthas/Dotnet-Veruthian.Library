using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Collections
{
    public class DataPool<A, V> : IPool<A, V>
    {
        Dictionary<A, (A, V)> items = new Dictionary<A, (A, V)>();


        public int Count => items.Count;

        bool IContainer<(A, V)>.Contains((A, V) value) => items.ContainsValue(value);

        public IEnumerator<(A, V)> GetEnumerator() => items.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => items.Values.GetEnumerator();

        public bool HasAddress(A address) => items.ContainsKey(address);

        public (A, V) Resolve(A address, V attribute = default(V))
        {
            if (!items.TryGetValue(address, out var pair))
            {
                pair = (address, attribute);

                items.Add(address, pair);
            }

            return pair;
        }

        public override string ToString() => this.ToTableString();
    }

    public class DataPool<A> : DataPool<A, object> { }
}