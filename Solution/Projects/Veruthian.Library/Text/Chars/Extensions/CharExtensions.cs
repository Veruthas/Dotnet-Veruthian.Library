using System.Collections.Generic;
using System.Text;
using Veruthian.Library.Text.Encodings;
using Veruthian.Library.Text.Lines;

namespace Veruthian.Library.Text.Chars.Extensions
{
    public static class CharExtensions
    {
        // Printable Chars
        public static string ToPrintableString(this char value)
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

        public static string ToPrintableString(this string value)
        {
            var builder = new StringBuilder();

            foreach (char c in value)
                builder.Append(ToPrintableString(c));

            return builder.ToString();
        }

        // LineTable
        public static IEnumerable<char> ProcessLines(this IEnumerable<char> values, out CharLineTable lines)
        {
            lines = new CharLineTable();

            return ProcessLines(values, lines);
        }

        private static IEnumerable<char> ProcessLines(this IEnumerable<char> values, CharLineTable lines)
        {
            foreach (var rune in values)
            {
                lines.Append(rune);

                yield return rune;
            }
        }

        // Lines
        public static IEnumerable<(int LineNumber, LineEnding Ending, string value)> GetLineData(this IEnumerable<char> values, LineEnding ending, bool keepEnding = true)
        {
            return LineEnding.GetLineData(values, ending, keepEnding, new StringBuffer(), (c => (uint)c), (b => b.ToString()));
        }

        public static IEnumerable<string> GetLines(this IEnumerable<char> values, LineEnding ending, bool keepEnding = true)
        {
            return LineEnding.GetLines(values, ending, keepEnding, new StringBuffer(), (c => (uint)c), (b => b.ToString()));
        }


        // String extensions
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsNullOrWhitespace(this string value) => string.IsNullOrWhiteSpace(value);
    }
}