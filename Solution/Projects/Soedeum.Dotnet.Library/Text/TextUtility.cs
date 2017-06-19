using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Soedeum.Dotnet.Library.Text
{
    public static class TextUtility
    {
        // Char Extensions
        public static bool IsIn(this char value, CharSet set)
        {
            return set.Contains(value);
        }

        // Enumerators
        public static IEnumerator<char> GetCharEnumerator(this TextReader reader)
        {
            while (true)
            {
                int read = reader.Read();

                if (read == -1)
                    break;
                else
                    yield return (char)read;
            }

            reader.Dispose();
        }

        public static IEnumerator<char> GetCharEnumerator(this Stream stream, Encoding encoding = null)
        {
            var reader = GetTextReader(stream, encoding);

            return GetCharEnumerator(reader);
        }

        public static IEnumerator<char> GetCharEnumerator(string filepath, Encoding encoding = null)
        {
            var reader = GetTextReader(filepath, encoding);

            return GetCharEnumerator(reader);
        }


        // TextElements
        public static IEnumerator<TextElement> GetTextElements(this IEnumerator<char> chars)
        {
            return TextElement.EnumerateFromChars(chars);
        }

        public static IEnumerator<TextElement> GetTextElements(this IEnumerable<char> chars)
        {
            return TextElement.EnumerateFromChars(chars.GetEnumerator());
        }


        // TextReaders
        public static TextReader GetTextReader(this Stream stream, Encoding encoding = null)
        {
            return (encoding == null) ? new StreamReader(stream) : new StreamReader(stream, encoding);
        }

        public static TextReader GetTextReader(string filepath, Encoding encoding = null)
        {
            var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);

            return GetTextReader(stream, encoding);
        }

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


        // LineEndings
        public static string GetAsString(this LineEnding ending)
        {
            switch (ending)
            {
                case LineEnding.Null:
                    return "\0";
                case LineEnding.Lf:
                    return "\n";
                case LineEnding.Cr:
                    return "\r";
                case LineEnding.CrLf:
                    return "\r\n";
                default:
                    return "\0";
            }
        }

        public static int GetSize(this LineEnding ending)
        {
            switch (ending)
            {
                case LineEnding.Null:
                    return 1;
                case LineEnding.Lf:
                case LineEnding.Cr:
                    return 1;
                case LineEnding.CrLf:
                    return 2;
                default:
                    return 0;
            }
        }

        public static string GetAsPrintable(this LineEnding ending)
        {
            switch (ending)
            {
                case LineEnding.Null:
                    return "\\0";
                case LineEnding.Lf:
                    return "\\n";
                case LineEnding.Cr:
                    return "\\r";
                case LineEnding.CrLf:
                    return "\\r\\n";
                default:
                    return "\\0";
            }
        }

        public static string GetShortName(this LineEnding ending)
        {
            switch (ending)
            {
                case LineEnding.Null:
                    return "Null";
                case LineEnding.Lf:
                    return "Lf";
                case LineEnding.Cr:
                    return "Cr";
                case LineEnding.CrLf:
                    return "CrLf";
                default:
                    return "?";
            }
        }

        public static string GetLongName(this LineEnding ending)
        {
            switch (ending)
            {
                case LineEnding.Null:
                    return "Null";
                case LineEnding.Lf:
                    return "LineFeed";
                case LineEnding.Cr:
                    return "CarriageReturn";
                case LineEnding.CrLf:
                    return "CarriageReturnLineFeed";
                default:
                    return "Unknown";
            }
        }


        public static LineEnding GetLineEnding(string ending)
        {
            switch (ending)
            {
                case "\n":
                    return LineEnding.Lf;
                case "\r":
                    return LineEnding.Cr;
                case "\r\n":
                    return LineEnding.CrLf;
                case "\0":
                    return LineEnding.Null;
                default:
                    return LineEnding.Null;
            }
        }
    }
}