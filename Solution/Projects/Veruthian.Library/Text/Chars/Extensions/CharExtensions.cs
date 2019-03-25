using System.Collections.Generic;
using System.Text;

namespace Veruthian.Library.Text.Chars.Extensions
{
    public static class CharExtensions
    {
        // Printable Chars
        public static string AsPrintable(this char value)
        {
            switch (value)
            {
                case '\n':
                    return "\\n";
                case '\r':
                    return "\\r";
                case '\t':
                    return "\\t";
                case '\0':
                    return "\\0";
                default:
                    return value.ToString();
            }
        }

        public static string AsPrintable(this string value)
        {
            var builder = new StringBuilder();

            foreach (char c in value)
                builder.Append(AsPrintable(c));

            return builder.ToString();
        }

        // LineTracking
        public static IEnumerable<char> ProcessLines(this IEnumerable<char> chars, out CharLineTable lines)
        {
            lines = new CharLineTable();

            return ProcessLines(chars, lines);
        }

        private static IEnumerable<char> ProcessLines(this IEnumerable<char> chars, CharLineTable lines)
        {            
            foreach (var rune in chars)
            {
                lines.Append(rune);

                yield return rune;
            }
        }


        // String extensions
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsNullOrWhitespace(this string value) => string.IsNullOrWhiteSpace(value);
    }
}