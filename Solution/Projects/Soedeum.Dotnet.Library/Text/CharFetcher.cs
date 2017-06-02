using System.IO;
using System.Text;
using Soedeum.Dotnet.Library.Collections;

namespace Soedeum.Dotnet.Library.Text
{
    public class CharFetcher : BaseFetcher<char>
    {
        TextReader reader;


        public CharFetcher(TextReader reader) => this.reader = reader;


        public override void Dispose() => reader.Dispose();

        public override bool IsEnd(char item) => item == '\0';


        public override char FetchInitial()
        {
            var initial = reader.Read();

            return (initial == -1) ? '\0' : (char)initial;
        }

        public override char FetchNext(char previous)
        {
            var next = reader.Read();

            return (next == -1) ? '\0' : (char)next;
        }




        public static CharFetcher FromString(string data)
        {
            return new CharFetcher(new StringReader(data));
        }

        public static CharFetcher FromStream(Stream stream, Encoding encoding = null)
        {
            var reader = (encoding == null) ? new StreamReader(stream) : new StreamReader(stream, encoding);

            return new CharFetcher(reader);
        }

        public static CharFetcher FromFile(string filepath, Encoding encoding = null)
        {
            var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);

            return FromStream(stream, encoding);
        }
    }
}