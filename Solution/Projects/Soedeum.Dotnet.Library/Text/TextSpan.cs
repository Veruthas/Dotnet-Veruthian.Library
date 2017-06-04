using System;
using System.Collections;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Text
{
    public struct TextSpan : IEnumerable<TextElement>
    {
        readonly TextPosition position;

        readonly string value;


        public TextSpan(string value)
            : this(new TextPosition(), value) { }

        public TextSpan(TextPosition position, string value)
        {
            this.position = position;

            this.value = value;
        }

        public TextPosition Position => position;

        public string Value => String.IsNullOrEmpty(value) ? string.Empty : value;


        public override string ToString()
        {
            return string.Format("{0}; Value: \"{1}\"", Position, TextHelper.GetStringAsPrintable(Value));
        }

        public IEnumerator<TextElement> GetEnumerator()
        {
            if (string.IsNullOrEmpty(value))
                yield break;

            var element = new TextElement(position, value[0]);

            yield return element;

            for (int i = 1; i < value.Length; i++)
            {
                element += value[i];

                yield return element;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}