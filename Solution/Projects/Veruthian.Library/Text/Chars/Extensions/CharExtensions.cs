using System.Collections.Generic;
using System.Text;

namespace Veruthian.Library.Text.Chars.Extensions
{
    public static class CharExtensions
    {
        // Printable Chars
        public static string GetAsPrintable(this char value)
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

        public static string GetAsPrintable(this string value)
        {
            var builder = new StringBuilder();

            foreach (char c in value)
                builder.Append(GetAsPrintable(c));

            return builder.ToString();
        }

        // LineTracking
        public static IEnumerable<char> ProcessLines(this IEnumerable<char> runes, out CharLineTable lines)
        {
            lines = new CharLineTable();

            return ProcessLines(runes, lines);
        }

        private static IEnumerable<char> ProcessLines(this IEnumerable<char> runes, CharLineTable lines)
        {
            char current = '\0';

            foreach (var rune in runes)
            {
                lines.MoveToNext(current, rune);

                yield return rune;

                current = rune;
            }

            lines.MoveToNext(current, '\0');
        }


        // String extensions
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsNullOrWhitespace(this string value) => string.IsNullOrWhiteSpace(value);
    }
}