using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Collections
{
    public class Enumerables
    {
        #region Empty

        private static class EmptyEnumerable<T>
        {
            public static IEnumerable<T> GetEnumerable()
            {
                yield break;
            }

            public static readonly IEnumerable<T> Default = GetEnumerable();
        }

        public static IEnumerable<T> GetEmpty<T>() => EmptyEnumerable<T>.Default;

        #endregion

        #region Ranges

        public static IEnumerable<int> GetRange(int start, int end)
        {
            for (int i = start; i <= end; i++)
                yield return i;
        }

        public static IEnumerable<long> GetRange(long start, long end)
        {
            for (long i = start; i <= end; i++)
                yield return i;
        }

        #endregion
    }
}