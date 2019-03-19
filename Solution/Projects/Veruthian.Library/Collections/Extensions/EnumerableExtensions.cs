using System.Collections.Generic;
using System.Text;

namespace Veruthian.Library.Collections.Extensions
{
    public static class EnumerableExtensions
    {
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