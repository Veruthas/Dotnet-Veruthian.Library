using System;
using System.Text;
using Veruthian.Library.Collections;
using Veruthian.Library.Text.Runes.Extensions;

namespace Veruthian.Library.Text.Runes
{
    public class RuneString : BaseString<Rune, RuneString>
    {
        protected override RuneString This => this;

        public static implicit operator RuneString(string value) => From(value.ToRuneArray());

        public static implicit operator string (RuneString value) => From(value.ToString());

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder((int)Count);

            foreach(var rune in this)
            {
                builder.Append(rune.ToString());
            }

            return builder.ToString();
        }

        internal static RuneString Remove(RuneString value, int position, int amount)
        {
            throw new NotImplementedException();
        }

        internal static RuneString Insert(RuneString value, int position, RuneString runeString)
        {
            throw new NotImplementedException();
        }
    }
}