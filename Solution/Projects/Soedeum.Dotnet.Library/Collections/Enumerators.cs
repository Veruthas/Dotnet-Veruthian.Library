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


        // Simple Scanner
        public static SimpleScanner<T> GetSimpleScanner<T>(this IEnumerator<T> enumerator,
                                                             Func<T, T> generateEndItem = null,
                                                             Action<SimpleScanner<T>, T> onItemRead = null)
        {
            var scanner = new SimpleScanner<T>(enumerator, generateEndItem);

            if (onItemRead != null)
                scanner.ItemRead += onItemRead;

            return scanner;
        }

        public static SimpleScanner<T> GetSimpleScanner<T>(this IEnumerable<T> enumerable,
                                                                     Func<T, T> generateEndItem = null,
                                                                     Action<SimpleScanner<T>, T> onItemRead = null)
        {
            return GetSimpleScanner(enumerable.GetEnumerator(), generateEndItem, onItemRead);
        }

        // Fixed Lookahead Scanner
        public static FixedLookaheadScanner<T> GetFixedLookaheadScanner<T>(this IEnumerator<T> enumerator,
                                                                                     int lookahead = 2,
                                                                                     Func<T, T> generateEndItem = null,
                                                                                     Action<FixedLookaheadScanner<T>, T> onItemRead = null)
        {
            var scanner = new FixedLookaheadScanner<T>(enumerator, lookahead, generateEndItem);

            if (onItemRead != null)
                scanner.ItemRead += onItemRead;

            return scanner;
        }

        public static FixedLookaheadScanner<T> GetFixedLookaheadScanner<T>(this IEnumerable<T> enumerable,
                                                                                     int lookahead = 2,
                                                                                     Func<T, T> generateEndItem = null,
                                                                                     Action<FixedLookaheadScanner<T>, T> onItemRead = null)
        {
            return GetFixedLookaheadScanner(enumerable.GetEnumerator(), lookahead, generateEndItem, onItemRead);
        }

        // Variable Lookahead Scanner
        public static VariableLookaheadScanner<T> GetVariableLookaheadScanner<T>(this IEnumerator<T> enumerator,                                                                                     
                                                                                     Func<T, T> generateEndItem = null,
                                                                                     Action<VariableLookaheadScanner<T>, T> onItemRead = null)
        {
            var scanner = new VariableLookaheadScanner<T>(enumerator, generateEndItem);

            if (onItemRead != null)
                scanner.ItemRead += onItemRead;

            return scanner;
        }

        public static VariableLookaheadScanner<T> GetVariableLookaheadScanner<T>(this IEnumerable<T> enumerable,
                                                                                     Func<T, T> generateEndItem = null,
                                                                                     Action<VariableLookaheadScanner<T>, T> onItemRead = null)
        {
            return GetVariableLookaheadScanner(enumerable.GetEnumerator(), generateEndItem, onItemRead);
        }
    }
}