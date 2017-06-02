using System.Text;
using Soedeum.Dotnet.Library.Collections;

namespace Soedeum.Dotnet.Library.Text
{
    public class TextLineFetcher : BaseFetcher<TextLine>
    {
        IScanner<char> stream;

        StringBuilder buffer = new StringBuilder();


        public TextLineFetcher(IScanner<char> stream) => this.stream = stream;

        public override void Dispose() => stream.Dispose();
        public override bool IsEnd(TextLine item) => item.Line == "\0";


        public override TextLine FetchInitial()
        {
            TextLine fetchedLine = new TextLine(stream, buffer);

            return fetchedLine;
        }

        public override TextLine FetchNext(TextLine previous)
        {
            TextLine fetchedLine = new TextLine(stream, previous, buffer);

            return fetchedLine;
        }   
    }
}