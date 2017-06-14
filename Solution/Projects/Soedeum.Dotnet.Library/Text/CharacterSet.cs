using System;
using System.Collections.Generic;
using System.Text;

namespace Soedeum.Dotnet.Library.Text
{
    public abstract class CharacterSet
    {
        private CharacterSet() { }

        public abstract bool Includes(char value);


        // Sets
        public static readonly CharacterSet All = AllCharacters.Defualt;

        public static readonly CharacterSet None = NoCharacters.Defualt;


        // Constructors
        public static implicit operator CharacterSet(char value) => FromValue(value);

        public static implicit operator CharacterSet(char[] list) => FromList(list);

        public static CharacterSet FromValue(char value) => new Value(value);

        public static CharacterSet FromList(params char[] list) => (list == null || list.Length == 0) ? None : new List(list);

        public static CharacterSet FromRange(char lowest, char highest)
        {
            if (lowest == highest)
                return FromValue(lowest);

            if (lowest > highest)
                Swap(ref lowest,  ref highest);

            if (lowest == char.MinValue && highest == char.MaxValue)
                return All;
            else
                return new Range(lowest, highest);
        }

        public static CharacterSet FromUnion(CharacterSet sets)
        {
            return null;
        }
        
        // Helper
        private struct Pair
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


            public override string ToString() => (Length == 1) ? GetString(Lowest) : GetString(this);
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

            public static readonly AllCharacters Defualt = new AllCharacters();
        }

        private class NoCharacters : CharacterSet
        {
            public override bool Includes(char value) => false;

            public override string ToString() => "<none>";

            public static readonly NoCharacters Defualt = new NoCharacters();
        }

        private class Value : CharacterSet
        {
            public char value;

            public Value(char value) => this.value = value;

            public override bool Includes(char value) => this.value == value;


            public override string ToString() => GetString(value);
        }

        private class Range : CharacterSet
        {
            public Pair region;


            public Range(char lowest, char highest) => this.region = new Pair(lowest, highest);

            public Range(Pair region) => this.region = region;

            public override bool Includes(char value) => region.Contains(value);

            public override string ToString() => region.ToString();
        }

        private class List : CharacterSet
        {
            public SortedSet<char> list;


            public List() => this.list = new SortedSet<char>();

            public List(IEnumerable<char> list) => this.list = new SortedSet<char>(list);


            public override bool Includes(char value) => this.list.Contains(value);

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
        }

        private class Union : CharacterSet
        {
            // Use a binary search through regions (a single char a => (a, a))
            public Pair[] ranges;


            public Union(Pair[] ranges) => this.ranges = ranges;


            public override bool Includes(char value)
            {
                // TODO: Binary search
                return false;
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