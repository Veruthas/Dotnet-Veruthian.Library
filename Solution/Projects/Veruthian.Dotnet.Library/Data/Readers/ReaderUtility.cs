using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public static class ReaderUtility
    {
        // Simple Reader
        public static Reader<T> GetReader<T>(this IEnumerator<T> enumerator,
                                                             GenerateEndItem<T> generateEndItem = null)
        {
            var reader = new Reader<T>(enumerator, generateEndItem);

            return reader;
        }

        public static Reader<T> GetReader<T>(this IEnumerable<T> enumerable,
                                                            GenerateEndItem<T> generateEndItem = null)
        {
            return GetReader(enumerable.GetEnumerator(), generateEndItem);
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

        public static VariableLookaheadReaderBase<T> GetVariableLookaheadReader<T>(this IEnumerable<T> enumerable,
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