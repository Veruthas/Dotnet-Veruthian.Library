using System;
using System.Collections;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public static class Enumerators
    {
        public static IScanner<T> GetSimpleScanner<T>(this IEnumerator<T> enumerator)
        {
            return null;
        }

        public static IScanner<T> GetSimpleScanner<T>(this IEnumerable<T> enumerable)
        {
            return GetSimpleScanner(enumerable.GetEnumerator());
        }


        public static ILookaheadScanner<T> GetLookaheadScanner<T>(this IEnumerator<T> enumerator, int lookahead = 2)
        {
            return null;
        }

        public static ILookaheadScanner<T> GetLookaheadScanner<T>(this IEnumerable<T> enumerable, int lookahead = 2)
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

        public static IEnumerable<T> GetEnumerable<T>(this IEnumerator<T> enumerator)
        {
            return new Enumerable<T>(enumerator);
        }
    }
}