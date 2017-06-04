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

        // NotifyinEnumerator
        public static NotifyingEnumerator<T> GetNotifyingEnumerator<T>(this IEnumerator<T> enumerator)
        {
            return new NotifyingEnumerator<T>(enumerator);
        }

        public static NotifyingEnumerator<T> GetNotifyingEnumerator<T>(this IEnumerator<T> enumerator, params Action<NotifyingEnumerator<T>, bool, T>[] notifyees)
        {
            var notifyer = new NotifyingEnumerator<T>(enumerator);

            foreach (var notifyee in notifyees)
                notifyer.MovedNext += notifyee;

            return notifyer;
        }

        // Scanners
        public static EnumeratorSimpleScanner<T> GetSimpleScanner<T>(this IEnumerator<T> enumerator)
        {
            return new EnumeratorSimpleScanner<T>(enumerator);
        }

        public static EnumeratorSimpleScanner<T> GetSimpleScanner<T>(this IEnumerable<T> enumerable)
        {
            return GetSimpleScanner(enumerable.GetEnumerator());
        }


        public static ILookaheadScanner<T> GetLookaheadScanner<T>(this IEnumerator<T> enumerator, int lookahead = 1)
        {
            return null;
        }

        public static ILookaheadScanner<T> GetLookaheadScanner<T>(this IEnumerable<T> enumerable, int lookahead = 1)
        {
            return GetLookaheadScanner(enumerable.GetEnumerator(), lookahead);
        }


        public static ILookaheadScanner<T> GetSpeculativeScanner<T>(this IEnumerator<T> enumerator)
        {
            return null;
        }

        public static ILookaheadScanner<T> GetSpeculativeScanner<T>(this IEnumerable<T> enumerable)
        {
            return GetSpeculativeScanner(enumerable.GetEnumerator());
        }
    }
}