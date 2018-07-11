using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Collections
{
    public class Enumerables
    {
        private static class EmptyEnumerable<T> 
        {
            public static IEnumerable<T> GetEnumerable()
            {
                yield break;
            }

            public static readonly IEnumerable<T> Default = GetEnumerable();
        }

        public IEnumerable<T> GetEmpty<T>() => EmptyEnumerable<T>.Default;
    }
}