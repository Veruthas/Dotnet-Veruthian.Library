using System;
using System.Collections.Generic;
using System.Text;

namespace Soedeum.Dotnet.Library.Text
{
    public abstract class CharSet
    {
        public abstract bool Includes(char value);

        // Sets
        public static readonly CharSet All = AllSet.Default;

        public static readonly CharSet Nothing = NothingSet.Default;

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


        // Implementations
        private class AllSet : CharSet
        {
            public override bool Includes(char value) => true;

            public static readonly AllSet Default = new AllSet();
        }

        private class NothingSet : CharSet
        {
            public override bool Includes(char value) => false;

            public static readonly NothingSet Default = new NothingSet();
        }


        private class CharValue : CharSet
        {
            public char value;

            public CharValue(char value)
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

        private class CharRange : CharSet
        {
            public char lowest, highest;

            public CharRange(char lowest, char highest)
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

        private class CharList : CharSet
        {
            public SortedSet<char> values = new SortedSet<char>();

            public CharList()
            {

            }

            public CharList(IEnumerable<char> list)
            {
                Add(list);
            }

            public void Add(IEnumerable<char> list)
            {
                foreach (char value in list)
                    this.values.Add(value);
            }

            public void Add(char value)
            {
                values.Add(value);
            }

            public override bool Includes(char value)
            {
                return values.Contains(value);
            }

            public override string ToString()
            {
                if (values.Count == 0)
                    return string.Empty;

                bool started = false;

                StringBuilder builder = new StringBuilder();

                foreach (char c in values)
                {
                    if (started)
                        builder.Append(" | ");

                    builder.Append("'")
                           .Append(TextUtility.GetAsPrintable(c))
                           .Append("'");

                    started = true;
                }

                return builder.ToString();
            }
        }

        private class CharSetComplement : CharSet
        {
            CharSet set;

            public CharSetComplement(CharSet set)
            {
                this.set = set ?? Nothing;
            }

            public override bool Includes(char value)
            {
                return !set.Includes(value);
            }

            public override string ToString()
            {
                return "not (" + set.ToString() + ")";
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

                buffer.Append(string.Format("{0}", sets[0]));

                for (int i = 1; i < sets.Length; i++)
                    buffer.Append(string.Format(" | {0}", sets[i]));

                return buffer.ToString();
            }
        }

        // Implicit converters
        public static implicit operator CharSet(char value) => FromValue(value);


        public static implicit operator CharSet(char[] list) => FromList(list);


        public static implicit operator CharSet(CharSet[] sets) => FromUnion(sets);

        // Static creators
        public static CharSet FromValue(char value) => new CharValue(value);

        public static CharSet FromRange(char lowest, char highest) => new CharRange(lowest, highest);

        public static CharSet FromList(params char[] list) => new CharList(list);

        public static CharSet FromComplement(CharSet set) => new CharSetComplement(set);

        public static CharSet FromUnion(params CharSet[] sets) => OptimizeCharSetUnion(sets);


        // Todo: make optimizer more efficient (overlapping ranges, compliment elimination, etc)
        private class CharSetOptimizer
        {
            List<CharRange> ranges;

            List<CharSetComplement> compliments;

            CharList list;

            CharValue value;

            int length;

            private void ProcessSets(CharSet[] sets)
            {
                foreach (var set in sets)
                {
                    if (set is CharRange)
                        AddRange(set as CharRange);
                    else if (set is CharList)
                        AddList(set as CharList);
                    else if (set is CharValue)
                        AddValue(set as CharValue);
                    else if (set is CharSetComplement)
                        AddCompliment(set as CharSetComplement);
                    else if (set is CharSetUnion)
                        AddUnion(set as CharSetUnion);
                }
            }

            public void AddUnion(CharSetUnion union)
            {
                ProcessSets(union.sets);
            }

            public void AddCompliment(CharSetComplement compliment)
            {
                if (compliments == null)
                    compliments = new List<CharSetComplement>();

                compliments.Add(compliment);

                length++;
            }

            public void AddRange(CharRange range)
            {
                if (ranges == null)
                {
                    ranges = new List<CharRange>();

                    ranges.Add(range);

                    length++;
                }
                else
                {
                    // Todo: optimize
                    // 'A' to 'F'
                    // 'A' to 'Z'
                    // 'B' to 'G'
                    ranges.Add(range);

                    length++;
                }
            }

            public void AddList(CharList list)
            {
                if (this.list != null)
                {
                    this.list.values.UnionWith(list.values);
                }
                else
                {
                    this.list = new CharList(list.values);

                    if (this.value != null)
                    {
                        list.Add(this.value.value);
                        this.value = null;
                    }
                    else
                    {
                        length++;
                    }
                }
            }

            public void AddValue(CharValue value)
            {
                if (this.list != null)
                {
                    this.list.values.Add(value.value);
                }
                else if (this.value != null)
                {
                    this.list = new CharList();
                    this.list.Add(this.value.value);
                    this.list.Add(value.value);
                    this.value = null;
                }
                else
                {
                    this.value = value;

                    length++;
                }
            }

            public CharSet GetOptimized(CharSet[] sets)
            {
                ProcessSets(sets);

                if (compliments == null && ranges == null)
                {
                    return list != null ? (CharSet)list : (CharSet)value;
                }
                else
                {
                    if (list != null)
                    {
                        list.values.RemoveWhere((c) => CharIncluded(c));

                        if (list.values.Count == 0)
                        {
                            list = null;
                            length--;
                        }
                    }
                    else if (value != null)
                    {
                        if (CharIncluded(value.value))
                        {
                            value = null;
                            length--;
                        }
                    }

                    sets = new CharSet[length];

                    int index = 0;

                    if (compliments != null)
                        foreach (var compliment in compliments)
                            sets[index++] = compliment;

                    if (ranges != null)
                        foreach (var range in ranges)
                            sets[index++] = range;

                    if (list != null)
                        sets[index] = list;
                    else if (value != null)
                        sets[index] = value;

                    return new CharSetUnion(sets);
                }
            }

            private bool CharIncluded(char value)
            {
                if (ranges != null)
                {
                    foreach (var range in ranges)
                        if (range.Includes(value))
                            return true;
                }

                if (compliments != null)
                {
                    foreach (var compliment in compliments)
                        if (compliment.Includes(value))
                            return true;
                }

                return false;
            }

            public static CharSet Optimize(CharSet[] sets)
            {
                var optimizer = new CharSetOptimizer();

                return optimizer.GetOptimized(sets);
            }
        }

        private static CharSet OptimizeCharSetUnion(CharSet[] sets)
        {
            return CharSetOptimizer.Optimize(sets);
        }
    }
}