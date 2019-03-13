using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Veruthian.Library.Text.Extensions
{
    public static class TextReaderExtensions
    {
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
    }
}