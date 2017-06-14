using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Soedeum.Dotnet.Library.Text
{
    public abstract class CharacterSet
    {
        private CharacterSet() { }

        public abstract bool Includes(char value);

        protected abstract void AddPairs(ICollection<Pair> pairs);

        // Sets
        public static readonly CharacterSet All = AllCharacters.Default;

        public static readonly CharacterSet None = NoCharacters.Defualt;


        // Constructors
        public static implicit operator CharacterSet(char value) => FromValue(value);

        public static implicit operator CharacterSet(char[] list) => FromList(list);

        public static CharacterSet FromValue(char value) => new Value(value);

        public static CharacterSet FromList(params char[] list)
        {
            if (list == null || list.Length == 0)
                return None;
            else if (list.Length == 1)
                return FromValue(list[0]);
            else
                return CheckForRanges(list);
        }

        private static CharacterSet CheckForRanges(char[] list)
        {
            List newlist = new List(list);

            List<Pair> pairs = new List<Pair>();


            int maxLength = 0;


            bool started = false;

            char lowest = '\0';

            char last = '\0';


            foreach (char value in newlist)
            {
                if (!started)
                {
                    started = true;
                    lowest = last = value;
                }
                else
                {
                    // Add a range if the two chars are not consecutive
                    if (last + 1 != value)
                    {
                        maxLength = Math.Max(maxLength, (last - lowest) + 1);

                        pairs.Add(new Pair(lowest, last));

                        lowest = value;
                    }

                    last = value;
                }
            }

            // Need to add range for remaining char(s)
            maxLength = Math.Max(maxLength, (last - lowest) + 1);

            pairs.Add(new Pair(lowest, last));

            // If range is of all characters, return All
            if (pairs.Count == 1)
            {
                var pair = pairs[0];

                if (pair.Lowest == char.MinValue && pair.Highest == char.MaxValue)
                    return All;
            }

            // If there are any ranges in the list return a union instead of a list
            if (maxLength > 1)
                return new Union(pairs.ToArray());
            else
                return newlist;
        }

        public static CharacterSet FromRange(char lowest, char highest)
        {
            if (lowest == highest)
                return FromValue(lowest);

            else if (lowest > highest)
                //Swap(ref lowest, ref highest);
                return None;

            if (lowest == char.MinValue && highest == char.MaxValue)
                return All;
            else
                return new Range(lowest, highest);
        }

        private static CharacterSet FromRange(Pair pair) => FromRange(pair.Lowest, pair.Highest);

        public static CharacterSet FromUnion(params CharacterSet[] sets)
        {
            if (sets == null || sets.Length == 0)
                return None;
            else if (sets.Length == 1)
                return sets[0];
            else
            {
                SortedSet<Pair> pairs = new SortedSet<Pair>();

                foreach (var set in sets)

                    if (set is AllCharacters)
                        return set;
                    else
                        set.AddPairs(pairs);

                Pair[] ranges = MergePairs(pairs);

                if (ranges.Length == 0)
                    return None;
                else if (ranges.Length == 1)
                    return FromRange(ranges[0]);
                else
                    return new Union(ranges);
            }
        }

        private static Pair[] MergePairs(SortedSet<Pair> pairs)
        {
            List<Pair> merged = new List<Pair>(pairs);

            if (merged.Count < 2)
                return merged.ToArray();

            
        }

        // Helper
        protected struct Pair : IEquatable<Pair>, IComparable<Pair>, IEnumerable<char>
        {
            public char Lowest, Highest;

            public Pair(char value)
            {
                Lowest = Highest = value;
            }

            public Pair(char lowest, char highest)
            {
                this.Lowest = lowest;

                this.Highest = highest;
            }

            public int Length => (Highest - Lowest) + 1;

            public bool Contains(char value) => (value >= Lowest) && (value <= Highest);

            public int CompareTo(Pair other)
            {
                if (this.Lowest > other.Lowest)
                    return 1;
                else if (this.Highest > other.Highest)
                    return 1;
                else if (this.Highest < other.Highest)
                    return -1;
                else
                    return 0;
            }

            public bool Equals(Pair other) => (this.Lowest == other.Lowest) && (this.Highest == other.Highest);


            public override string ToString() => (Length == 1) ? GetString(Lowest) : GetString(this);

            public IEnumerator<char> GetEnumerator()
            {
                char current = Lowest;

                while (current <= Highest)
                {
                    yield return current;
                    current++;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private static string GetString(char value) => string.Format("'{0}'", TextUtility.GetAsPrintable(value));

        private static string GetString(Pair pair) => string.Format("('{0}' to '{1}')", TextUtility.GetAsPrintable(pair.Lowest), TextUtility.GetAsPrintable(pair.Highest));

        private static void Swap(ref char a, ref char b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        // Classes
        private class AllCharacters : CharacterSet
        {
            public override bool Includes(char value) => true;

            public override string ToString() => "<all>";

            protected sealed override void AddPairs(ICollection<Pair> pairs) => pairs.Add(new Pair(char.MinValue, char.MaxValue));

            public static readonly AllCharacters Default = new AllCharacters();
        }

        private class NoCharacters : CharacterSet
        {
            public override bool Includes(char value) => false;

            public override string ToString() => "<none>";

            protected override void AddPairs(ICollection<Pair> pairs) { }

            public static readonly NoCharacters Defualt = new NoCharacters();
        }

        private class Value : CharacterSet
        {
            public char value;

            public Value(char value) => this.value = value;

            public sealed override bool Includes(char value) => this.value == value;


            public override string ToString() => GetString(value);

            protected override void AddPairs(ICollection<Pair> pairs) => pairs.Add(new Pair(value, value));
        }

        private class Range : CharacterSet
        {
            public Pair region;


            public Range(char lowest, char highest) => this.region = new Pair(lowest, highest);

            public Range(Pair region) => this.region = region;

            public override bool Includes(char value) => region.Contains(value);

            protected override void AddPairs(ICollection<Pair> pairs) => pairs.Add(region);

            public override string ToString() => region.ToString();
        }

        private class List : CharacterSet, IEnumerable<char>
        {
            public SortedSet<char> list;


            public List() => this.list = new SortedSet<char>();

            public List(IEnumerable<char> list) => this.list = new SortedSet<char>(list);


            public override bool Includes(char value) => this.list.Contains(value);

            protected sealed override void AddPairs(ICollection<Pair> pairs)
            {
                foreach (char value in list)
                    pairs.Add(new Pair(value, value));
            }

            public void Add(char value) => this.list.Add(value);

            public void Add(IEnumerable<char> list) => this.list.UnionWith(list);


            public override string ToString()
            {
                StringBuilder buffer = new StringBuilder();

                bool initalized = false;

                foreach (char value in list)
                {
                    if (initalized)
                        buffer.Append(" | ");
                    else
                        initalized = true;

                    buffer.Append(GetString(value));
                }

                return buffer.ToString();
            }

            public IEnumerator<char> GetEnumerator() => list.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        }

        private class Union : CharacterSet
        {
            // Use a binary search through regions (a single char a => (a, a))
            public Pair[] ranges;


            public Union(Pair[] ranges) => this.ranges = ranges;


            public override bool Includes(char value) => Find(value) != -1;

            protected sealed override void AddPairs(ICollection<Pair> pairs)
            {
                foreach (var range in ranges)
                    pairs.Add(range);
            }
            public int Find(char value)
            {
                int low = 0;
                int high = ranges.Length - 1;

                while (low < high)
                {
                    int current = (high - low) / 2;

                    var range = ranges[current];

                    if (value < range.Lowest)
                        high = current - 1;
                    else if (value > range.Highest)
                        low = current + 1;
                    else
                        return current;
                }

                return -1;
            }

            public override string ToString()
            {
                StringBuilder buffer = new StringBuilder();

                bool initalized = false;

                foreach (var range in ranges)
                {
                    if (initalized)
                        buffer.Append(" | ");
                    else
                        initalized = true;

                    buffer.Append(range.ToString());
                }

                return buffer.ToString();
            }
        }
    }
}