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

            notifyer.MovedNext += onMoveNextHandler;

            return notifyer;
        }

        public static NotifyingEnumerator<T> GetNotifyingEnumerator<T>(this IEnumerable<T> enumerable, Action<NotifyingEnumerator<T>, bool, T> onMoveNextHandler = null)
        {
            return GetNotifyingEnumerator(enumerable.GetEnumerator(), onMoveNextHandler);
        }


        // Simple Reader
        public static SimpleReader<T> GetSimpleReader<T>(this IEnumerator<T> enumerator,
                                                             Func<T, T> generateEndItem = null,
                                                             Action<IReader<T>, T> onItemRead = null)
        {
            var scanner = new SimpleReader<T>(enumerator, generateEndItem);

            scanner.ItemRead += onItemRead;

            return scanner;
        }

        public static SimpleReader<T> GetSimpleReader<T>(this IEnumerable<T> enumerable,
                                                                     Func<T, T> generateEndItem = null,
                                                                     Action<IReader<T>, T> onItemRead = null)
        {
            return GetSimpleReader(enumerable.GetEnumerator(), generateEndItem, onItemRead);
        }

        // Fixed Lookahead Reader
        public static FixedLookaheadReader<T> GetFixedLookaheadReader<T>(this IEnumerator<T> enumerator,
                                                                                     int lookahead = 2,
                                                                                     Func<T, T> generateEndItem = null,
                                                                                     Action<IReader<T>, T> onItemRead = null)
        {
            var scanner = new FixedLookaheadReader<T>(enumerator, lookahead, generateEndItem);

            scanner.ItemRead += onItemRead;

            return scanner;
        }

        public static FixedLookaheadReader<T> GetFixedLookaheadReader<T>(this IEnumerable<T> enumerable,
                                                                                     int lookahead = 2,
                                                                                     Func<T, T> generateEndItem = null,
                                                                                     Action<IReader<T>, T> onItemRead = null)
        {
            return GetFixedLookaheadReader(enumerable.GetEnumerator(), lookahead, generateEndItem, onItemRead);
        }

        // Variable Lookahead Reader
        public static VariableLookaheadReader<T> GetVariableLookaheadReader<T>(this IEnumerator<T> enumerator,
                                                                                     Func<T, T> generateEndItem = null,
                                                                                     Action<IReader<T>, T> onItemRead = null)
        {
            var scanner = new VariableLookaheadReader<T>(enumerator, generateEndItem);

            scanner.ItemRead += onItemRead;

            return scanner;
        }

        public static VariableLookaheadReader<T> GetVariableLookaheadReader<T>(this IEnumerable<T> enumerable,
                                                                                     Func<T, T> generateEndItem = null,
                                                                                     Action<IReader<T>, T> onItemRead = null)
        {
            return GetVariableLookaheadReader(enumerable.GetEnumerator(), generateEndItem, onItemRead);
        }

        // SpeculativeReader
        public static SpeculativeReader<T> GetSpeculativeReader<T>(
                                                                this IEnumerator<T> enumerator,
                                                                Func<T, T> generateEndItem = null,
                                                                Action<IReader<T>, T> onItemRead = null,
                                                                Action<ISpeculativeReader<T>> onSpeculating = null,
                                                                Action<ISpeculativeReader<T>> onCommitted = null,
                                                                Action<ISpeculativeReader<T>, int, int> onRetracted = null)
        {
            var scanner = new SpeculativeReader<T>(enumerator, generateEndItem);

            scanner.ItemRead += onItemRead;

            scanner.Speculating += onSpeculating;

            scanner.Committed += onCommitted;

            scanner.Retracted += onRetracted;

            return scanner;
        }

        public static SpeculativeReader<T> GetSpeculativeReader<T>(
                                                                this IEnumerable<T> enumerable,
                                                                Func<T, T> generateEndItem = null,
                                                                Action<IReader<T>, T> onItemRead = null,
                                                                Action<ISpeculativeReader<T>> onSpeculating = null,
                                                                Action<ISpeculativeReader<T>> onCommitted = null,
                                                                Action<ISpeculativeReader<T>, int, int> onRetracted = null)
        {
            return GetSpeculativeReader(enumerable.GetEnumerator(), generateEndItem, onItemRead, onSpeculating, onCommitted, onRetracted);
        }


        // Speculative Reader w/State
        public static SpeculativeReaderWithState<T, S> GetSpeculativeReaderWithState<T, S>(
                                                                this IEnumerator<T> enumerator,
                                                                Func<T, T> generateEndItem = null,
                                                                Action<IReader<T>, T> onItemRead = null,
                                                                Func<ISpeculativeReader<T>, S> onSpeculating = null,
                                                                Action<ISpeculativeReader<T>> onCommitted = null,
                                                                Action<ISpeculativeReader<T>, int, int, S> onRetracted = null)
        {
            var scanner = new SpeculativeReaderWithState<T, S>(enumerator, generateEndItem);

            scanner.ItemRead += onItemRead;

            scanner.Speculating += onSpeculating;

            scanner.Committed += onCommitted;

            scanner.Retracted += onRetracted;

            return scanner;
        }

        public static SpeculativeReaderWithState<T, S> GetSpeculativeReaderWithState<T, S>(
                                                                this IEnumerable<T> enumerable,
                                                                Func<T, T> generateEndItem = null,
                                                                Action<IReader<T>, T> onItemRead = null,
                                                                Func<ISpeculativeReader<T>, S> onSpeculating = null,
                                                                Action<ISpeculativeReader<T>> onCommitted = null,
                                                                Action<ISpeculativeReader<T>, int, int, S> onRetracted = null)
        {
            return GetSpeculativeReaderWithState(enumerable.GetEnumerator(), generateEndItem, onItemRead, onSpeculating, onCommitted, onRetracted);
        }
    }
}