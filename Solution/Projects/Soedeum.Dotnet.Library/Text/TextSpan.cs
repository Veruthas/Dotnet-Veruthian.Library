using System;
using System.Collections;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Text
{
    public struct TextSpan : IEnumerable<TextElement>
    {
        TextPosition position;

        string text;


        public TextSpan(string text) : this(default(TextPosition), text)
        {
        }

        public TextSpan(TextPosition position, string text)
        {
            this.position = position;
            this.text = text;
        }

        public TextPosition Position { get => position; }

        public string Text { get => text ?? string.Empty; }

        public IEnumerator<TextElement> GetEnumerator(bool acceptNulls)
        {
            return TextElement.EnumerateFromChars(position, text, acceptNulls);
        }

        public IEnumerator<TextElement> GetEnumerator()
        {
            return TextElement.EnumerateFromChars(position, text);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}