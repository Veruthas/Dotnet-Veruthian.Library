using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public static class EnumeratorUtility
    {
        // Stream Enumerator
        public static IEnumerator<byte> GetEnumerator(this Stream stream)
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

        // Adapter
        public static EnumerableAdapter<T> GetEnumerableAdapter<T>(this IEnumerator<T> enumerator)
        {
            return new EnumerableAdapter<T>(enumerator);
        }

        // NotifyingEnumerator
        public static NotifyingEnumerator<T> GetNotifyingEnumerator<T>(this IEnumerator<T> enumerator, EnumeratorMoveNext<T> onMoveNext = null)
        {
            var notifyer = new NotifyingEnumerator<T>(enumerator);

            notifyer.MovedNext += onMoveNext;

            return notifyer;
        }

        public static NotifyingEnumerator<T> GetNotifyingEnumerator<T>(this IEnumerable<T> enumerable, EnumeratorMoveNext<T> onMoveNext = null)
        {
            return GetNotifyingEnumerator(enumerable.GetEnumerator(), onMoveNext);
        }       
    }
}