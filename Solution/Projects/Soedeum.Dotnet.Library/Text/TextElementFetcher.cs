using Soedeum.Dotnet.Library.Collections;

namespace Soedeum.Dotnet.Library.Text
{
    public class TextElementFetcher : BaseFetcher<TextElement>
    {
        IScanner<char> stream;

        public TextElementFetcher(IScanner<char> stream) => this.stream = stream;

        public override void Dispose() => stream.Dispose();
        public override bool IsEnd(TextElement item) => item.Value == '\0';

        public override TextElement FetchInitial()
        {
            var value = stream.Consume();

            var element = new TextElement(value);

            return element;
        }

        public override TextElement FetchNext(TextElement previous)
        {
            var value = stream.Consume();

            previous += value;

            return previous;
        }
    }
}