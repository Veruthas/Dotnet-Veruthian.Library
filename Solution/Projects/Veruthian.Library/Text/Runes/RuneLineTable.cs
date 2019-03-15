using Veruthian.Library.Text.Lines;

namespace Veruthian.Library.Text.Runes
{
    public class RuneLineTable : BaseLineTable<Rune, RuneString>
    {
        protected sealed override uint ConvertToUtf32(Rune value) => value;

        protected sealed override int GetLength(RuneString value) => value.Length;

        protected sealed override RuneString Slice(RuneString value, int start, int length) => value.Slice(start, length);
    }
}