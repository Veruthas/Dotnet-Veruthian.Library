using System;
using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Enumeration
{
    public struct EnumerableAdapter<T> : IEnumerable<T>
    {
        IEnumerator<T> enumerator;

        public EnumerableAdapter(IEnumerator<T> enumerator) => this.enumerator = enumerator;

        public IEnumerator<T> GetEnumerator()
        {
            if (this.enumerator == null)
                throw new InvalidOperationException("Enumerator has already been used.");

            var enumerator = this.enumerator;

            this.enumerator = null;

            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}