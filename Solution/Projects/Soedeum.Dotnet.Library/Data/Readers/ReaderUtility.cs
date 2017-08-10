using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Data.Readers
{
    public static class ReaderUtility
    {
        // Simple Reader
        public static SimpleReader<T> GetSimpleReader<T>(this IEnumerator<T> enumerator,
                                                             GenerateEndItem<T> generateEndItem = null)
        {
            var reader = new SimpleReader<T>(enumerator, generateEndItem);

            return reader;
        }

        public static SimpleReader<T> GetSimpleReader<T>(this IEnumerable<T> enumerable,
                                                            GenerateEndItem<T> generateEndItem = null)
        {
            return GetSimpleReader(enumerable.GetEnumerator(), generateEndItem);
        }

        // Fixed Lookahead Reader
        public static FixedLookaheadReader<T> GetFixedLookaheadReader<T>(this IEnumerator<T> enumerator,
                                                                            int lookahead = 2,
                                                                            GenerateEndItem<T> generateEndItem = null)
        {
            var reader = new FixedLookaheadReader<T>(enumerator, lookahead, generateEndItem);

            return reader;
        }

        public static FixedLookaheadReader<T> GetFixedLookaheadReader<T>(this IEnumerable<T> enumerable,
                                                                            int lookahead = 2,
                                                                            GenerateEndItem<T> generateEndItem = null)
        {
            return GetFixedLookaheadReader(enumerable.GetEnumerator(), lookahead, generateEndItem);
        }

        // Variable Lookahead Reader
        public static VariableLookaheadReader<T> GetVariableLookaheadReader<T>(this IEnumerator<T> enumerator,
                                                                                GenerateEndItem<T> generateEndItem = null)
        {
            var reader = new VariableLookaheadReader<T>(enumerator, generateEndItem);

            return reader;
        }

        public static VariableLookaheadReader<T> GetVariableLookaheadReader<T>(this IEnumerable<T> enumerable,
                                                                                GenerateEndItem<T> generateEndItem = null)
        {
            return GetVariableLookaheadReader(enumerable.GetEnumerator(), generateEndItem);
        }

        // Speculative Reader
        public static SpeculativeReader<T> GetSpeculativeReader<T>(
                                                                this IEnumerator<T> enumerator,
                                                                GenerateEndItem<T> generateEndItem = null)
        {
            var reader = new SpeculativeReader<T>(enumerator, generateEndItem);

            return reader;
        }

        public static SpeculativeReader<T> GetSpeculativeReader<T>(
                                                            this IEnumerable<T> enumerable,
                                                            GenerateEndItem<T> generateEndItem = null)
        {
            return GetSpeculativeReader(enumerable.GetEnumerator(), generateEndItem);
        }
    }
}