using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Collections
{
    public class DataPool<A, D> : IPool<A, D>
    {
        Dictionary<A, (A, D)> items = new Dictionary<A, (A, D)>();


        public int Count => items.Count;

        bool IContainer<(A, D)>.Contains((A, D) value) => items.ContainsValue(value);

        public IEnumerator<(A, D)> GetEnumerator() => items.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => items.Values.GetEnumerator();

        public bool HasAddress(A address) => items.ContainsKey(address);

        public (A, D) Resolve(A address, D data = default(D))
        {
            if (!items.TryGetValue(address, out var pair))
            {
                pair = (address, data);

                items.Add(address, pair);
            }

            return pair;
        }

        public override string ToString() => this.ToTableString();
    }

    public class DataPool<A> : DataPool<A, object> { }
}