using Veruthian.Library.Text.Lines;

namespace Veruthian.Library.Text.Chars
{
    public class CharLineTable : BaseLineTable<char>
    {
        public CharLineTable() : base() { }

        public CharLineTable(LineEnding endingType) : base(endingType) { }

        protected sealed override uint ConvertToUtf32(char value) => value;
    }
}