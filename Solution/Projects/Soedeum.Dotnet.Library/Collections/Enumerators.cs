using System;
using System.Collections;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public static class Enumerators
    {
        public static Enumerable<T> GetEnumerable<T>(this IEnumerator<T> enumerator)
        {
            return new Enumerable<T>(enumerator);
        }

        // NotifyingEnumerator
        public static NotifyingEnumerator<T> GetNotifyingEnumerator<T>(this IEnumerator<T> enumerator, Action<NotifyingEnumerator<T>, bool, T> onMoveNextHandler = null)
        {
            var notifyer = new NotifyingEnumerator<T>(enumerator);

            if (onMoveNextHandler != null)
                notifyer.MovedNext += onMoveNextHandler;

            return notifyer;
        }

        public static NotifyingEnumerator<T> GetNotifyingEnumerator<T>(this IEnumerable<T> enumerable, Action<NotifyingEnumerator<T>, bool, T> onMoveNextHandler = null)
        {
            return GetNotifyingEnumerator(enumerable.GetEnumerator(), onMoveNextHandler);
        }


        // SimpleScanner
        public static EnumeratorSimpleScanner<T> GetSimpleScanner<T>(this IEnumerator<T> enumerator,
                                                             Func<T, T> getEndItem = null,
                                                             Action<EnumeratorSimpleScanner<T>, T> onItemRead = null)
        {
            var scanner = new EnumeratorSimpleScanner<T>(enumerator, getEndItem);

            if (onItemRead != null)
                scanner.ItemRead += onItemRead;

            return scanner;
        }

        public static EnumeratorSimpleScanner<T> GetSimpleScanner<T>(this IEnumerable<T> enumerable,
                                                                     Func<T, T> getEndItem = null,
                                                                     Action<EnumeratorSimpleScanner<T>, T> onItemRead = null)
        {
            return GetSimpleScanner(enumerable.GetEnumerator(), getEndItem, onItemRead);
        }
    }
}