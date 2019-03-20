using System.Collections.Generic;
using System.Text;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Collections.Extensions
{
    public static class EnumerableExtensions
    {
        // ToArray
        public static T[] ToArray<T>(this IEnumerable<T> items, int size)
        {
            ExceptionHelper.VerifyPositive(size, nameof(size));

            var newArray = new T[size];

            int index = 0;

            foreach (var item in items)
            {
                if (index < size)
                    newArray[index++] = item;
                else
                    break;
            }

            return newArray;
        }


        // Extract
        public static IEnumerable<T> Extract<T>(this IEnumerable<T> items, int amount)
        {
            ExceptionHelper.VerifyPositive(amount, nameof(amount));

            var index = 0;

            foreach (var item in items)
            {
                if (index < amount)
                    yield return item;
                else
                    break;
            }

            for (; index < amount; index++)
                yield return default(T);
        }


        // Range
        public static IEnumerable<int> To(this int start, int end, int step = 1) => Enumerables.GetRange(start, end, step);

        public static IEnumerable<long> To(this long start, long end, long step = 1) => Enumerables.GetRange(start, end, step);


        // Strings
        public static string ToListString<T>(this IEnumerable<T> items, string start = "[", string end = "]", string separator = ", ")
        {
            var builder = new StringBuilder();

            builder.Append(start);

            bool started = false;

            foreach (var item in items)
            {
                if (started)
                    builder.Append(separator);
                else
                    started = true;

                builder.Append(item.ToString());
            }

            builder.Append(end);

            return builder.ToString();
        }

        public static string ToTableString<K, V>(this IEnumerable<(K, V)> items, string tableStart = "{", string tableEnd = "}",
                                                                                 string pairStart = "", string pairEnd = "", string pairSeparator = ",",
                                                                                 string keyStart = "[", string keyEnd = "] = ",
                                                                                 string valueStart = "'", string valueEnd = "'")
        {
            var builder = new StringBuilder();


            bool started = false;

            builder.Append(tableStart);

            foreach ((K key, V value) in items)
            {
                if (started)
                    builder.Append(pairSeparator);
                else
                    started = true;

                builder.Append(pairStart);

                // Key
                builder.Append(keyStart);
                builder.Append(key);
                builder.Append(keyEnd);

                // Value
                builder.Append(valueStart);
                builder.Append(value);
                builder.Append(valueEnd);

                builder.Append(pairEnd);
            }

            builder.Append(tableEnd);

            return builder.ToString();
        }
    }
}