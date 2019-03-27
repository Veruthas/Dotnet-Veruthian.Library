using System.Collections.Generic;
using Veruthian.Library.Text.Lines;

namespace Veruthian.Library.Text.Chars
{
    public class CharLineTable<S> : BaseLineTable<char, S>
        where S : IEnumerable<char>
    {
        public CharLineTable() : base() { }

        public CharLineTable(LineEnding endingType) : base(endingType) { }

        protected sealed override uint ConvertToUtf32(char value) => value;
    }

    public class CharLineTable: CharLineTable<IEnumerable<char>>    
    {
        public CharLineTable() : base() { }

        public CharLineTable(LineEnding endingType) : base(endingType) { }
    }
}