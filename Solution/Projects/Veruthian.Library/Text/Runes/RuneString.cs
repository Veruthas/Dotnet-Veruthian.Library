using System;
using System.Collections.Generic;
using System.Text;
using Veruthian.Library.Collections;
using Veruthian.Library.Numeric;
using Veruthian.Library.Text.Runes.Extensions;
using Veruthian.Library.Utility.Extensions;

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


        public static RuneString operator +(RuneString left, RuneString right)
            => left.Appended(right);

        public static RuneString operator +(RuneString left, Rune right)
            => left.Appended(right);

        public static RuneString operator +(RuneString left, Rune[] right)
            => left.Appended(right);

        public static RuneString operator +(RuneString left, IEnumerable<Rune> right)
            => left.Appended(right);

        public static RuneString operator +(Rune left, RuneString right)
            => right.Prepended(left);

        public static RuneString operator +(Rune[] left, RuneString right)
            => right.Prepended(left);

        public static RuneString operator +(IEnumerable<Rune> left, RuneString right)
            => right.Prepended(left);



        public static RuneString operator *(RuneString left, Number right)
            => left.Repeated(right);

        public static RuneString operator ~(RuneString left)
            => left.Reversed();


        public static bool operator ==(RuneString left, RuneString right)
            => Equals(left, right);

        public static bool operator !=(RuneString left, RuneString right)
            => !Equals(left, right);

        public static bool operator >(RuneString left, RuneString right)
            => left.IsNotNull() && Compare(left, right) > 0;

        public static bool operator <(RuneString left, RuneString right)
            => left.IsNotNull() && Compare(left, right) < 0;

        public static bool operator >=(RuneString left, RuneString right)
            => left.IsNotNull() && Compare(left, right) >= 0;

        public static bool operator <=(RuneString left, RuneString right)
            => left.IsNotNull() && Compare(left, right) <= 0;


        public override bool Equals(object other) => base.Equals(other);

        public override int GetHashCode() => base.GetHashCode();
    }
}