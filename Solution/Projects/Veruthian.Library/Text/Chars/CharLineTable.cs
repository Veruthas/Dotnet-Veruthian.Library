using Veruthian.Library.Text.Lines;

namespace Veruthian.Library.Text.Chars
{
    public class CharLineTable : BaseLineTable<char, string>
    {
        protected sealed override uint ConvertToUtf32(char value) => value;

        protected sealed override int GetLength(string value) => value.Length;

        protected sealed override string Slice(string value, int start, int length) => value.Substring(start, length);
    }
}