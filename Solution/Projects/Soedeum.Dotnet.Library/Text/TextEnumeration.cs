using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Soedeum.Dotnet.Library.Text
{
    public static class TextEnumeration
    {
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
            var reader = GetReaderFromStream(stream, encoding);

            return FromTextReader(reader);
        }

        public static IEnumerator<char> FromFile(string filepath, Encoding encoding = null)
        {
            var reader = GetReaderFromFile(filepath, encoding);

            return FromTextReader(reader);
        }


        public static TextReader GetReaderFromStream(Stream stream, Encoding encoding = null)
        {
            return (encoding == null) ? new StreamReader(stream) : new StreamReader(stream, encoding);
        }

        public static TextReader GetReaderFromFile(string filepath, Encoding encoding = null)
        {
            var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);

            return GetReaderFromStream(stream, encoding);
        }
    }
}