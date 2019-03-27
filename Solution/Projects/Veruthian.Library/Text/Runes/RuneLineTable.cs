using System.Collections.Generic;
using Veruthian.Library.Text.Lines;

namespace Veruthian.Library.Text.Runes
{
    public class RuneLineTable<S> : BaseLineTable<Rune, S>
        where S : IEnumerable<Rune>
    {
        public RuneLineTable() : base() { }

        public RuneLineTable(LineEnding endingType) : base(endingType) { }

        protected sealed override uint ConvertToUtf32(Rune value) => value;
    }

    public class RuneLineTable : RuneLineTable<IEnumerable<Rune>>
    {
        public RuneLineTable() : base() { }

        public RuneLineTable(LineEnding endingType) : base(endingType) { }
    }

}