using Soedeum.Dotnet.Library.Collections;

namespace Soedeum.Dotnet.Library.Text
{
    public class TextElementFetcher : BaseFetcher<TextElement>
    {
        IFetcher<char> chars;

        public TextElementFetcher(IFetcher<char> chars) => this.chars = chars;

        public override void Dispose() => chars.Dispose();
        public override bool IsEnd(TextElement item) => item.Value == '\0';

        public override TextElement FetchInitial()
        {
            var value = chars.FetchInitial();

            var element = new TextElement(value);

            return element;
        }

        public override TextElement FetchNext(TextElement previous)
        {
            var value = chars.FetchNext(previous.Value);

            previous += value;

            return previous;
        }
    }
}