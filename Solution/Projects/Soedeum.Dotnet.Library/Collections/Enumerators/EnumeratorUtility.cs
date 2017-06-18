using System;
using System.Collections;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public static class EnumeratorUtility
    {
        public static Enumerable<T> GetEnumerable<T>(this IEnumerator<T> enumerator)
        {
            return new Enumerable<T>(enumerator);
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


        // Simple Reader
        public static SimpleReader<T> GetSimpleReader<T>(this IEnumerator<T> enumerator,
                                                             GenerateEndItem<T> generateEndItem = null,
                                                             ReaderRead<T> onItemRead = null)
        {
            var reader = new SimpleReader<T>(enumerator, generateEndItem);

            reader.ItemRead += onItemRead;

            return reader;
        }

        public static SimpleReader<T> GetSimpleReader<T>(this IEnumerable<T> enumerable,
                                                            GenerateEndItem<T> generateEndItem = null,
                                                            ReaderRead<T> onItemRead = null)
        {
            return GetSimpleReader(enumerable.GetEnumerator(), generateEndItem, onItemRead);
        }

        // Fixed Lookahead Reader
        public static FixedLookaheadReader<T> GetFixedLookaheadReader<T>(this IEnumerator<T> enumerator,
                                                                            int lookahead = 2,
                                                                            GenerateEndItem<T> generateEndItem = null,
                                                                            ReaderRead<T> onItemRead = null)
        {
            var reader = new FixedLookaheadReader<T>(enumerator, lookahead, generateEndItem);

            reader.ItemRead += onItemRead;

            return reader;
        }

        public static FixedLookaheadReader<T> GetFixedLookaheadReader<T>(this IEnumerable<T> enumerable,
                                                                            int lookahead = 2,
                                                                            GenerateEndItem<T> generateEndItem = null,
                                                                            ReaderRead<T> onItemRead = null)
        {
            return GetFixedLookaheadReader(enumerable.GetEnumerator(), lookahead, generateEndItem, onItemRead);
        }

        // Variable Lookahead Reader
        public static VariableLookaheadReader<T> GetVariableLookaheadReader<T>(this IEnumerator<T> enumerator,
                                                                                GenerateEndItem<T> generateEndItem = null,
                                                                                ReaderRead<T> onItemRead = null)
        {
            var reader = new VariableLookaheadReader<T>(enumerator, generateEndItem);

            reader.ItemRead += onItemRead;

            return reader;
        }

        public static VariableLookaheadReader<T> GetVariableLookaheadReader<T>(this IEnumerable<T> enumerable,
                                                                                GenerateEndItem<T> generateEndItem = null,
                                                                                ReaderRead<T> onItemRead = null)
        {
            return GetVariableLookaheadReader(enumerable.GetEnumerator(), generateEndItem, onItemRead);
        }

        // Speculative Reader
        public static SpeculativeReader<T> GetSpeculativeReader<T>(
                                                                this IEnumerator<T> enumerator,
                                                                GenerateEndItem<T> generateEndItem = null,
                                                                ReaderRead<T> onItemRead = null,
                                                                SpeculationIncident<T> onMarked = null,
                                                                SpeculationIncident<T> onCommitted = null,
                                                                SpeculationRetreated<T> onRetreated = null)
        {
            var reader = new SpeculativeReader<T>(enumerator, generateEndItem);

            reader.ItemRead += onItemRead;

            reader.Marked += onMarked;

            reader.Committed += onCommitted;

            reader.Retreated += onRetreated;

            return reader;
        }

        public static SpeculativeReader<T> GetSpeculativeReader<T>(
                                                            this IEnumerable<T> enumerable,
                                                            GenerateEndItem<T> generateEndItem = null,
                                                            ReaderRead<T> onItemRead = null,
                                                            SpeculationIncident<T> onMarked = null,
                                                            SpeculationIncident<T> onCommitted = null,
                                                            SpeculationRetreated<T> onRetreated = null)
        {
            return GetSpeculativeReader(enumerable.GetEnumerator(), generateEndItem, onItemRead, onMarked, onCommitted, onRetreated);
        }       
    }
}