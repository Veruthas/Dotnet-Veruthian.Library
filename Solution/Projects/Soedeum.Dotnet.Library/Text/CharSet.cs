using System;
using System.Collections.Generic;
using System.Text;

namespace Soedeum.Dotnet.Library.Text
{
    // TODO: calculate simplifications
    public abstract class CharSet
    {
        public abstract bool Includes(char value);


        private class CharSetValue : CharSet
        {
            char value;

            public CharSetValue(char value)
            {
                this.value = value;
            }

            public override bool Includes(char value)
            {
                return this.value == value;
            }

            public override string ToString()
            {
                return string.Format("'{0}'", value.GetAsPrintable());
            }
        }

        private class CharSetRange : CharSet
        {
            char lowest, highest;

            public CharSetRange(char lowest, char highest)
            {
                this.lowest = lowest;
                this.highest = highest;
            }

            public override bool Includes(char value)
            {
                return (value >= lowest && value <= highest);
            }

            public override string ToString()
            {
                return string.Format("'{0}'..'{1}'", lowest.GetAsPrintable(), highest.GetAsPrintable());
            }
        }

        private class CharSetList : CharSet
        {
            char[] list;

            public CharSetList(params char[] list)
            {
                Array.Sort(list);

                this.list = list;
            }

            public override bool Includes(char value)
            {
                return Array.BinarySearch(list, value) >= 0;
            }

            public override string ToString()
            {
                if (list.Length == 0)
                    return string.Empty;

                StringBuilder buffer = new StringBuilder();

                buffer.Append(string.Format("'{0}'", list[0].GetAsPrintable()));

                for (int i = 1; i < list.Length; i++)
                    buffer.Append(string.Format(" | '{0}'", list[i].GetAsPrintable()));

                return buffer.ToString();
            }
        }

        private class CharSetUnion : CharSet
        {
            CharSet[] sets;

            public CharSetUnion(params CharSet[] sets)
            {
                List<CharSet> flattened = new List<CharSet>();

                foreach (CharSet set in sets)
                    FlattenCharSet(flattened, set);

                this.sets = flattened.ToArray();
            }

            private void FlattenCharSet(List<CharSet> flattened, CharSet set)
            {
                if (set is CharSetUnion)
                {
                    var union = (CharSetUnion)set;

                    foreach (var subset in union.sets)
                        FlattenCharSet(flattened, subset);
                }
                else
                {
                    if (set != null)
                        flattened.Add(set);
                }
            }

            public override bool Includes(char value)
            {
                foreach (var set in sets)
                    if (set.Includes(value))
                        return true;

                return false;
            }

            public override string ToString()
            {
                if (sets.Length == 0)
                    return string.Empty;

                StringBuilder buffer = new StringBuilder();

                buffer.Append(string.Format("'{0}'", sets[0]));

                for (int i = 1; i < sets.Length; i++)
                    buffer.Append(string.Format(" | '{0}'", sets[i]));

                return buffer.ToString();
            }
        }

        public static CharSet FromValue(char value) => new CharSetValue(value);

        public static CharSet FromRange(char lowest, char highest) => new CharSetRange(lowest, highest);

        public static CharSet FromList(params char[] list) => new CharSetList(list);

        public static CharSet FromUnion(params CharSet[] sets) => new CharSetUnion(sets);


        public static implicit operator CharSet(char value)
        {
            return FromValue(value);
        }

        public static implicit operator CharSet(char[] list)
        {
            return FromList(list);
        }

        public static implicit operator CharSet(CharSet[] sets)
        {
            return FromUnion(sets);
        }

        public static readonly CharSet Null = '\0';

        public static readonly CharSet NewLine = FromList('\n', '\r');

        public static readonly CharSet SpaceOrTab = FromList(' ', '\t');

        public static readonly CharSet Whitespace = FromList(' ', '\t', '\n', '\r');

        public static readonly CharSet Lower = FromRange('a', 'z');

        public static readonly CharSet Upper = FromRange('A', 'Z');

        public static readonly CharSet Letter = FromUnion(Lower, Upper);

        public static readonly CharSet Digit = FromRange('0', '9');

        public static readonly CharSet LetterOrDigit = FromUnion(Letter, Digit);

        public static readonly CharSet HexDigit = FromUnion(Digit, FromRange('A', 'F'), FromRange('a', 'f'));

        public static readonly CharSet CStyleIndentifierStart = FromUnion(Letter, '_');

        public static readonly CharSet CStyleIndentifier = FromUnion(LetterOrDigit, '_');
    }
}