using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public struct FixedContainer<T> : IContainer<T>
    {
        IContainer<T> container;

        public FixedContainer(IContainer<T> container) => this.container = container;

        public Number Count => container.Count;

        public bool Contains(T value) => container.Contains(value);

        public IEnumerator<T> GetEnumerator() => container.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => container.GetEnumerator();

        public override string ToString() => container.ToListString();
    }
}