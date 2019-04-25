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


        // Line Table
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
        public static IEnumerable<TextSegment> GetLineSegments(this IEnumerable<char> values, LineEnding ending = null, bool withEnding = true)
        {
            return TextSegment.GetLineSegments(values, (c => (uint)c), ending, withEnding);
        }

        public static IEnumerable<(TextSegment Segment, string Value)> GetLineData(this IEnumerable<char> values, LineEnding ending = null, bool withEnding = true)
        {
            return TextSegment.GetLineData(values, (c => (uint)c), new EditableString(), (b => b.ToString()), ending, withEnding);
        }

        public static IEnumerable<string> GetLines(this IEnumerable<char> values, LineEnding ending = null, bool withEnding = true)
        {
            return TextSegment.GetLines(values, (c => (uint)c), new StringBuffer(), (b => b.ToString()), ending, withEnding);
        }

        public static IEnumerable<(TextSegment Segment, string Value)> GetLineData(this string values, LineEnding ending = null, bool withEnding = true)
        {
            return TextSegment.GetLineData<char, string>(values, c => (uint)c, (s, p, l) => s.Substring(p, l), ending, withEnding);
        }

        public static IEnumerable<string> GetLines(this string values, LineEnding ending = null, bool withEnding = true)
        {
            return TextSegment.GetLines<char, string>(values, c => (uint)c, (s, p, l) => s.Substring(p, l), ending, withEnding);
        }


        // String extensions
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsNullOrWhitespace(this string value) => string.IsNullOrWhiteSpace(value);
    }
}