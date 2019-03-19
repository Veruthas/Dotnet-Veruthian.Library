using System.Collections.Generic;
using System.IO;

namespace Veruthian.Library.Collections
{
    public static class Enumerables
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

        #region Notifying

        public delegate void MoveNext<T>(bool success, T current);

        public static IEnumerable<T> GetNotifyingEnumerable<T>(IEnumerable<T> enumerable, MoveNext<T> notify)
        {
            foreach(var item in enumerable)
            {
                if (notify != null)
                    notify(true, item);

                yield return item;
            }

            if (notify != null)
                notify(false, default(T));            
        }

        #endregion

        #region Stream

        public static IEnumerator<byte> GetStreamEnumerator(Stream stream)
        {
            while (true)
            {
                int value = stream.ReadByte();

                if (value != -1)
                    yield return (byte)value;
                else
                    yield break;
            }
        }

        #endregion

        #region Ranges

        public static IEnumerable<int> GetRange(int start, int end, int step = 1)
        {
            for (int i = start; i <= end; i += step)
                yield return i;
        }

        public static IEnumerable<long> GetRange(long start, long end, long step = 1)
        {
            for (long i = start; i <= end; i += step)
                yield return i;
        }

        #endregion

        #region Repeat

        public static IEnumerable<T> Repeat<T>(this T value, int times = 1)
        {
            for (int i = 0; i < times; i++)
                yield return value;
        }

        #endregion
    }
}