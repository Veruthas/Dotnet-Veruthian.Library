using System;
using System.Collections;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Data.Enumeration
{
    public class EnumeratorGenerator<T> : IEnumerable<T>
    {
        GetEnumerator<T> generator;

        public EnumeratorGenerator(GetEnumerator<T> generator) => this.generator = generator;

        public IEnumerator<T> GetEnumerator() => generator();

        IEnumerator IEnumerable.GetEnumerator() => generator();
    }

    public delegate IEnumerator<T> GetEnumerator<T>();
}