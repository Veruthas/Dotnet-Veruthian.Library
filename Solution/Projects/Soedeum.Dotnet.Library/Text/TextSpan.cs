using System;
using System.Collections;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Text
{
    public struct TextSpan : IEnumerable<TextElement>
    {
        TextLocation location;

        string text;


        public TextSpan(string text) : this(default(TextLocation), text)
        {
        }

        public TextSpan(TextLocation location, string text)
        {
            this.location = location;
            this.text = text;
        }

        public TextLocation Location { get => location; }

        public int Position { get => location.Position; }

        public int Line { get => location.Line; }

        public int Column { get => location.Column; }

        public string Text { get => text ?? string.Empty; }

        public IEnumerator<TextElement> GetEnumerator(bool acceptNulls)
        {
            return TextElement.EnumerateFromChars(location, text, acceptNulls);
        }

        public IEnumerator<TextElement> GetEnumerator()
        {
            return TextElement.EnumerateFromChars(location, text);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}