using Veruthian.Library.Text.Lines;

namespace Veruthian.Library.Text.Runes
{
    public class RuneLineTable : BaseLineTable<Rune>
    {
        public RuneLineTable() : base() { }

        public RuneLineTable(LineEnding endingType) : base(endingType) { }
        
        protected sealed override uint ConvertToUtf32(Rune value) => value;
    }
}