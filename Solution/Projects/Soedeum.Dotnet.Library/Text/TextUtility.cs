using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Soedeum.Dotnet.Library.Text
{
    public static class TextUtility
    {
        // Enumerators
        public static IEnumerator<char> FromTextReader(TextReader reader)
        {
            while (true)
            {
                int read = reader.Read();

                if (read == -1)
                    break;
                else
                    yield return (char)read;
            }
        }

        public static IEnumerator<char> FromStream(Stream stream, Encoding encoding = null)
        {
            var reader = GetTextReaderFromStream(stream, encoding);

            return FromTextReader(reader);
        }

        public static IEnumerator<char> FromFile(string filepath, Encoding encoding = null)
        {
            var reader = GetTextReaderFromFile(filepath, encoding);

            return FromTextReader(reader);
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
        public static TextReader GetTextReaderFromStream(Stream stream, Encoding encoding = null)
        {
            return (encoding == null) ? new StreamReader(stream) : new StreamReader(stream, encoding);
        }

        public static TextReader GetTextReaderFromFile(string filepath, Encoding encoding = null)
        {
            var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);

            return GetTextReaderFromStream(stream, encoding);
        }

        // Printable Chars
        public static string GetCharAsPrintable(char value)
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

        public static string GetStringAsPrintable(string value)
        {
            var builder = new StringBuilder();

            foreach (char c in value)
                builder.Append(GetCharAsPrintable(c));

            return builder.ToString();
        }


        // LineEndings
        public static string GetLineEndingAsString(LineEnding ending)
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

        public static int GetLineEndingSize(LineEnding ending)
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

        public static string GetLineEndingAsPrintable(LineEnding ending)
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

        public static string GetLineEndingShortName(LineEnding ending)
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

        public static string GetLineEndingLongName(LineEnding ending)
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
    }
}