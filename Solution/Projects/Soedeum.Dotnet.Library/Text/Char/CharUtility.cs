using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Soedeum.Dotnet.Library.Text.Char
{
    public static class CharUtility
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
    }
}