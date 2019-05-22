using Veruthian.Library.Collections;

namespace Veruthian.Library.Text.Chars
{
    public class StringBuffer : BaseResizableVector<char, StringBuffer>, IEditableText<char>
    {
        protected override StringBuffer This => this;

        void IEditableText<char>.Append(char value) => Append(value);

        void IEditableText<char>.Clear() => Clear();

        void IEditableText<char>.Insert(int position, char value) => Insert(position, value);

        void IEditableText<char>.Prepend(char value) => Prepend(value);

        void IEditableText<char>.Remove(int position, int amount) => RemoveBy(position, amount);
    }
}