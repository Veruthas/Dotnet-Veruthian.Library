using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Veruthian.Library.Text.Chars.Extensions
{
    public static class TextReaderExtensions
    {
        // Enumerators
        public static IEnumerator<char> GetChars(this TextReader reader)
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

        public static IEnumerator<char> GetChars(this Stream stream, Encoding encoding = null)
        {
            var reader = GetTextReader(stream, encoding);

            return GetChars(reader);
        }

        public static IEnumerator<char> GetChars(string filepath, Encoding encoding = null)
        {
            var reader = GetTextReader(filepath, encoding);

            return GetChars(reader);
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
    }
}