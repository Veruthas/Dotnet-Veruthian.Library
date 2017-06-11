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
            public char value;

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
            public char lowest, highest;

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
            public SortedSet<char> set = new SortedSet<char>();

            public CharSetList()
            {

            }

            public CharSetList(IEnumerable<char> list)
            {
                Add(list);
            }

            public void Add(IEnumerable<char> list)
            {
                foreach (char value in list)
                    set.Add(value);
            }

            public void Add(char value)
            {
                set.Add(value);
            }

            public override bool Includes(char value)
            {
                return set.Contains(value);
            }

            public override string ToString()
            {
                if (set.Count == 0)
                    return string.Empty;

                List<string> buffer = new List<String>();

                foreach (char c in set)
                    buffer.Add(c.GetAsPrintable());

                return string.Join(" | ", buffer);
            }
        }

        private class CharSetUnion : CharSet
        {
            public CharSet[] sets;

            public CharSetUnion(params CharSet[] sets)
            {
                this.sets = sets;
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

        public static CharSet FromUnion(params CharSet[] sets)
        {
            return OptimizeCharSetUnion(sets);
        }

        private static CharSet OptimizeCharSetUnion(CharSet[] sets)
        {
            List<CharSet> ranges = null;

            CharSet chars = null;

            bool newListCreated = false;

            foreach (CharSet set in sets)
                OptimizeCharSetUnion(set, ref ranges, ref chars, ref newListCreated);

            if (ranges == null)
            {
                return chars;
            }
            else
            {
                if (chars != null)
                    ranges.Add(chars);

                return new CharSetUnion(ranges.ToArray());
            }
        }

        private static void OptimizeCharSetUnion(CharSet set, ref List<CharSet> ranges, ref CharSet chars, ref bool newListCreated)
        {
            if (set is CharSetUnion)
            {
                var union = set as CharSetUnion;

                foreach (var subset in union.sets)
                    OptimizeCharSetUnion(set, ref ranges, ref chars, ref newListCreated);
            }
            else if (set is CharSetRange)
            {
                if (ranges == null)
                    ranges = new List<CharSet>();

                ranges.Add(set);
            }
            else if (set is CharSetList)
            {
                var list = set as CharSetList;

                if (chars == null)
                {
                    chars = list;
                }
                else if (chars is CharSetValue)
                {
                    char value = (chars as CharSetValue).value;

                    var newList = new CharSetList(list.set);

                    newList.Add(value);

                    chars = newList;
                }
                else
                {
                    var oldList = chars as CharSetList;

                    if (!newListCreated)
                    {
                        oldList = new CharSetList(oldList.set);
                        newListCreated = true;
                    }

                    oldList.Add(list.set);

                    chars = oldList;
                }
            }
            else if (set is CharSetValue)
            {
                chars = set;
            }
        }


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

        public static readonly CharSet Whitespace = FromUnion(SpaceOrTab, NewLine);

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