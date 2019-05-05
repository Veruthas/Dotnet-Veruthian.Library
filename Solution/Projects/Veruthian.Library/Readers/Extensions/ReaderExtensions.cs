using System.Collections.Generic;

namespace Veruthian.Library.Readers.Extensions
{
    public static class ReaderExtensions
    {
        // Simple Reader
        public static IReader<T> GetReader<T>(this IEnumerator<T> enumerator,
                                             GenerateEndItem<T> generateEndItem = null)
        {
            var reader = new Reader<T>(enumerator, generateEndItem);

            return reader;
        }

        public static IReader<T> GetReader<T>(this IEnumerable<T> enumerable,
                                             GenerateEndItem<T> generateEndItem = null)
        {
            return GetReader(enumerable.GetEnumerator(), generateEndItem);
        }

        // Fixed Lookahead Reader
        public static ILookaheadReader<T> GetFixedLookaheadReader<T>(this IEnumerator<T> enumerator,
                                                                         int lookahead = 2,
                                                                         GenerateEndItem<T> generateEndItem = null)
        {
            var reader = new FixedLookaheadReader<T>(enumerator, lookahead, generateEndItem);

            return reader;
        }

        public static ILookaheadReader<T> GetFixedLookaheadReader<T>(this IEnumerable<T> enumerable,
                                                                         int lookahead = 2,
                                                                         GenerateEndItem<T> generateEndItem = null)
        {
            return GetFixedLookaheadReader(enumerable.GetEnumerator(), lookahead, generateEndItem);
        }

        // Variable Lookahead Reader
        public static ILookaheadReader<T> GetVariableLookaheadReader<T>(this IEnumerator<T> enumerator,
                                                                               GenerateEndItem<T> generateEndItem = null)
        {
            var reader = new VariableLookaheadReader<T>(enumerator, generateEndItem);

            return reader;
        }

        public static ILookaheadReader<T> GetVariableLookaheadReader<T>(this IEnumerable<T> enumerable,
                                                                                   GenerateEndItem<T> generateEndItem = null)
        {
            return GetVariableLookaheadReader(enumerable.GetEnumerator(), generateEndItem);
        }

        // Speculative Reader
        public static ISpeculativeReader<T> GetSpeculativeReader<T>(this IEnumerator<T> enumerator, 
                                                                   GenerateEndItem<T> generateEndItem = null)
        {
            var reader = new SpeculativeReader<T>(enumerator, generateEndItem);

            return reader;
        }

        public static ISpeculativeReader<T> GetSpeculativeReader<T>(this IEnumerable<T> enumerable, 
                                                                   GenerateEndItem<T> generateEndItem = null)
        {
            return GetSpeculativeReader(enumerable.GetEnumerator(), generateEndItem);
        }


        // Recollective Reader
        public static IRecollectiveReader<T, K, V> GetRecollectiveReader<T, K, V>(this IEnumerator<T> enumerator, 
                                                                                 GenerateEndItem<T> generateEndItem = null)
        {
            return new RecollectiveReader<T, K, V>(enumerator, generateEndItem);
        }

        public static IRecollectiveReader<T, K, V> GetRecollectiveReader<T, K, V>(this IEnumerable<T> enumerable,
                                                                                 GenerateEndItem<T> generateEndItem = null)
        {
            return GetRecollectiveReader<T, K, V>(enumerable.GetEnumerator(), generateEndItem);
        }

        public static IRecollectiveReader<T, K, object> GetRecollectiveReader<T, K>(this IEnumerator<T> enumerator,
                                                                                    GenerateEndItem<T> generateEndItem = null)
        {
            return new RecollectiveReader<T, K>(enumerator, generateEndItem);
        }

        public static IRecollectiveReader<T, K, object> GetRecollectiveReader<T, K>(this IEnumerable<T> enumerable,
                                                                                    GenerateEndItem<T> generateEndItem = null)
        {
            return GetRecollectiveReader<T, K>(enumerable.GetEnumerator(), generateEndItem);
        }
    }
}