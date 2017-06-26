using System.Collections;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class EmptyEnumerable<T> : IEnumerable<T>
    {
        private EmptyEnumerable() { }

        public IEnumerator<T> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static readonly EmptyEnumerable<T> Default = new EmptyEnumerable<T>();    
    }
}