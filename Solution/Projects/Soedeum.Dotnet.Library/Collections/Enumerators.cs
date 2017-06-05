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


        // Simple Scanner
        public static EnumeratorSimpleScanner<T> GetSimpleScanner<T>(this IEnumerator<T> enumerator, Func<T, T> generateEndItem = null, Action<EnumeratorSimpleScanner<T>, T> onItemReadHandler = null)
        {
            var scanner = new EnumeratorSimpleScanner<T>(enumerator, generateEndItem);

            if (onItemReadHandler != null)
                scanner.ItemRead += onItemReadHandler;

            return scanner;
        }

        public static EnumeratorSimpleScanner<T> GetSimpleScanner<T>(this IEnumerable<T> enumerable, Func<T, T> generateEndItem = null, Action<EnumeratorSimpleScanner<T>, T> onItemReadHandler = null)
        {
            return GetSimpleScanner(enumerable.GetEnumerator(), generateEndItem, onItemReadHandler);
        }


        // Lookahead Scanner
        // public static EnumeratorLookaheadScanner<T> GetLookaheadScanner<T>(this IEnumerator<T> enumerator,
        //                                                                     int lookahead = 1,
        //                                                                     Func<T, T> generateEndItem = null,
        //                                                                     Action<EnumeratorLookaheadScanner<T>, T> onItemReadHandler = null)
        // {
        //     var scanner = new EnumeratorLookaheadScanner<T>(enumerator, lookahead, generateEndItem);

        //     if (onItemReadHandler != null)
        //         scanner.ItemRead += onItemReadHandler;

        //     return scanner;
        // }

        // public static EnumeratorLookaheadScanner<T> GetLookaheadScanner<T>(this IEnumerable<T> enumerable,
        //                                                                     int lookahead = 1,
        //                                                                     Func<T, T> generateEndItem = null,
        //                                                                     Action<EnumeratorLookaheadScanner<T>, T> onItemReadHandler = null)
        // {
        //     return GetLookaheadScanner(enumerable.GetEnumerator(), lookahead, generateEndItem, onItemReadHandler);
        // }


        // SpeculativeScanner
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