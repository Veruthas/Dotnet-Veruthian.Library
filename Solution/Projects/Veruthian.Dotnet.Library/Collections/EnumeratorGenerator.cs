using System;
using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Collections
{
    public class EnumeratorGenerator<T, TEnumerator> : IEnumerable<T>
        where TEnumerator : IEnumerator<T>
    {
        Func<TEnumerator> generator;

        protected EnumeratorGenerator() { }

        public IEnumerator<T> GetEnumerator() => generator();

        IEnumerator IEnumerable.GetEnumerator() => generator();


        public static EnumeratorGenerator<T, TEnumerator> Create(Func<TEnumerator> generator)
        {
            var result = new EnumeratorGenerator<T, TEnumerator>();

            result.generator = generator;

            return result;
        }
    }

    public class EnumeratorGenerator<T> : EnumeratorGenerator<T, IEnumerator<T>>
    {
        protected EnumeratorGenerator() { }
    }
}