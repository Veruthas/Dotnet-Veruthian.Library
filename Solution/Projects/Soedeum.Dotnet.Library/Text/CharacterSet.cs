using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Utility;

namespace Soedeum.Dotnet.Library.Text
{
    public abstract class CharacterSet : IEnumerable<char>, IEquatable<CharacterSet>
    {
        #region Base Class
        int hashcode;

        private CharacterSet(int hashcode)
        {
            this.hashcode = hashcode;
        }

        // Abstracts methods
        public abstract bool Includes(char value);

        public abstract int Size { get; }

        protected abstract void AddPairs(ICollection<Pair> pairs);

        protected abstract bool IsEqualTo(CharacterSet other);

        public abstract IEnumerator<char> GetEnumerator();


        // Implemented
        public override int GetHashCode() => hashcode;


        public override bool Equals(object other)
        {
            if (other is CharacterSet)
                return this.Equals(other as CharacterSet);
            else
                return false;
        }

        public bool Equals(CharacterSet other)
        {
            return IsNull(other) ? false : IsEqualTo(other);
        }


        public static bool operator ==(CharacterSet left, CharacterSet right)
        {
            return IsNull(left) ? false : left.Equals(right);
        }
        public static bool operator !=(CharacterSet left, CharacterSet right)
        {
            return !(left == right);
        }

        public static CharacterSet operator +(CharacterSet left, CharacterSet right) => Union(left, right);

        private static bool IsNull(object T) => T == null;


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion


        // Classes
        #region Classes
        protected struct Pair : IEquatable<Pair>, IComparable<Pair>, IEnumerable<char>
        {
            public readonly char Lowest, Highest;

            public Pair(char value)
            {
                Lowest = Highest = value;
            }

            public Pair(char lowest, char highest)
            {
                this.Lowest = lowest;

                this.Highest = highest;
            }

            public int Size => (Highest - Lowest) + 1;

            public bool Contains(char value) => (value >= Lowest) && (value <= Highest);

            public int CompareTo(Pair other)
            {
                if (this.Lowest < other.Lowest)
                    return -1;
                else if (this.Lowest > other.Lowest)
                    return 1;
                else if (this.Highest < other.Highest)
                    return -1;
                else if (this.Highest > other.Highest)
                    return 1;
                else
                    return 0;
            }

            public bool Equals(Pair other) => (this.Lowest == other.Lowest) && (this.Highest == other.Highest);

            public override bool Equals(object obj) => (obj is Pair) ? Equals((Pair)obj) : false;

            public static bool operator ==(Pair left, Pair right) => left.Equals(right);

            public static bool operator !=(Pair left, Pair right) => !left.Equals(right);

            public override int GetHashCode() => HashCodeCombiner.Combiner.Combine(Lowest, Highest);

            public override string ToString() => (Size == 1) ? GetString(Lowest) : GetString(this);

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

        private class CompleteSet : CharacterSet
        {
            public CompleteSet() : base(AsPair.GetHashCode()) { }

            public override bool Includes(char value) => true;

            public override int Size => AsPair.Size;

            protected override bool IsEqualTo(CharacterSet other) => other is CompleteSet;

            public override IEnumerator<char> GetEnumerator() => AsPair.GetEnumerator();

            public override string ToString() => "<all>";

            protected sealed override void AddPairs(ICollection<Pair> pairs) => pairs.Add(new Pair(char.MinValue, char.MaxValue));

            public static readonly CompleteSet Default = new CompleteSet();

            private static readonly Pair AsPair = new Pair(char.MinValue, char.MaxValue);
        }

        private class EmptySet : CharacterSet
        {
            public EmptySet() : base(0) { }


            public override bool Includes(char value) => false;

            public override int Size => 0;

            protected override bool IsEqualTo(CharacterSet other) => other is EmptySet;

            public override IEnumerator<char> GetEnumerator()
            {
                yield break;
            }

            public override string ToString() => "<none>";

            protected override void AddPairs(ICollection<Pair> pairs) { }

            public static readonly EmptySet Default = new EmptySet();
        }

        private sealed class ValueSet : CharacterSet
        {
            public char value;

            public ValueSet(char value) : base(value.GetHashCode()) => this.value = value;

            public override bool Includes(char value) => this.value == value;

            public override int Size => 1;

            protected override bool IsEqualTo(CharacterSet other) => (other is ValueSet) && (value == ((ValueSet)other).value);

            public override IEnumerator<char> GetEnumerator()
            {
                yield return value;
            }


            public override string ToString() => GetString(value);

            protected override void AddPairs(ICollection<Pair> pairs) => pairs.Add(new Pair(value, value));
        }

        private sealed class RangeSet : CharacterSet
        {
            public Pair range;


            public RangeSet(char lowest, char highest)
                : this(new Pair(lowest, highest)) { }

            public RangeSet(Pair range) : base(range.GetHashCode()) => this.range = range;

            public override bool Includes(char value) => range.Contains(value);

            public override int Size => range.Size;

            protected override bool IsEqualTo(CharacterSet other) => (other is RangeSet) && (this.range == ((RangeSet)other).range);

            public override IEnumerator<char> GetEnumerator() => range.GetEnumerator();


            protected override void AddPairs(ICollection<Pair> pairs) => pairs.Add(range);

            public override string ToString() => range.ToString();
        }

        private sealed class ListSet : CharacterSet
        {
            public SortedSet<char> list;


            public ListSet() : base(0) => this.list = new SortedSet<char>();

            public ListSet(IEnumerable<char> list) : this() => this.Add(list);


            public override bool Includes(char value) => this.list.Contains(value);

            public override int Size => list.Count;

            public override IEnumerator<char> GetEnumerator() => list.GetEnumerator();

            protected sealed override void AddPairs(ICollection<Pair> pairs)
            {
                foreach (char value in list)
                    pairs.Add(new Pair(value, value));
            }

            public void Add(char value)
            {
                this.list.Add(value);
                base.hashcode = HashCodeCombiner.Combiner.Combine(list);
            }

            public void Add(IEnumerable<char> list)
            {
                this.list.UnionWith(list);
                base.hashcode = HashCodeCombiner.Combiner.Combine(this.list);
            }

            protected override bool IsEqualTo(CharacterSet other)
            {
                if (other is ListSet)
                {
                    ListSet otherList = (ListSet)other;

                    if (this.hashcode == otherList.hashcode)
                        return this.list.SetEquals(otherList.list);
                }

                return false;
            }

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

        private sealed class UnionSet : CharacterSet
        {
            // Use a binary search through regions (a single char a => (a, a))
            public Pair[] ranges;

            int size = 0;

            public UnionSet(Pair[] ranges) : base(HashCodeCombiner.Combiner.Combine(ranges))
            {
                this.ranges = ranges;

                foreach (var range in ranges)
                    size += range.Size;
            }


            public override bool Includes(char value) => Find(value) != -1;

            public override int Size => size;

            public override IEnumerator<char> GetEnumerator()
            {
                foreach (var range in ranges)
                {
                    if (range.Size == 1)
                        yield return range.Lowest;
                    else
                        for (var current = range.Lowest; current <= range.Highest; current++)
                            yield return current;
                }
            }

            protected sealed override void AddPairs(ICollection<Pair> pairs)
            {
                foreach (var range in ranges)
                    pairs.Add(range);
            }

            public int Find(char value)
            {
                int low = 0;
                int high = ranges.Length - 1;

                while (low <= high)
                {
                    int current = (high + low) / 2;

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

            protected override bool IsEqualTo(CharacterSet other)
            {
                if (other is UnionSet)
                {
                    var union = other as UnionSet;

                    if (this.hashcode == union.hashcode)
                    {
                        if (this.ranges.Length == union.ranges.Length)
                        {
                            for (int i = 0; i < ranges.Length; i++)
                                if (this.ranges[i] != union.ranges[i])
                                    return false;

                            return true;
                        }
                    }
                }

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
        #endregion


        // Constructors
        #region Constructors

        public static implicit operator CharacterSet(char value) => Value(value);

        public static implicit operator CharacterSet(char[] list) => List(list);

        public static implicit operator CharacterSet(string list) => List(list);

        public static CharacterSet Value(char value) => new ValueSet(value);

        public static CharacterSet List(params char[] list)
        {
            if (list == null || list.Length == 0)
                return None;
            else if (list.Length == 1)
                return Value(list[0]);
            else
                return CheckForRanges(list);
        }

        public static CharacterSet List(string list)
        {
            if (string.IsNullOrEmpty(list))
                return None;
            else if (list.Length == 1)
                return Value(list[0]);
            else
                return CheckForRanges(list);
        }

        private static CharacterSet CheckForRanges(IEnumerable<char> list)
        {
            ListSet newlist = new ListSet(list);

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
                return new UnionSet(pairs.ToArray());
            else
                return newlist;
        }

        public static CharacterSet Range(char lowest, char highest)
        {
            if (lowest == highest)
                return Value(lowest);

            else if (lowest > highest)
                //Swap(ref lowest, ref highest);
                return None;

            if (lowest == char.MinValue && highest == char.MaxValue)
                return All;
            else
                return new RangeSet(lowest, highest);
        }

        private static CharacterSet Range(Pair pair)
        {
            return Range(pair.Lowest, pair.Highest);
        }

        public static CharacterSet Union(params CharacterSet[] sets)
        {
            if (sets == null || sets.Length == 0)
                return None;
            else if (sets.Length == 1)
                return sets[0];
            else
            {
                SortedSet<Pair> pairs = new SortedSet<Pair>();

                foreach (var set in sets)
                {
                    if (set is CompleteSet)
                        return set;
                    else
                        set.AddPairs(pairs);
                }

                Pair[] ranges = MergePairs(pairs);

                if (ranges.Length == 0)
                    return None;
                else if (ranges.Length == 1)
                    return Range(ranges[0]);
                else
                    return new UnionSet(ranges);
            }
        }

        private static Pair[] MergePairs(SortedSet<Pair> pairs)
        {
            List<Pair> merged = new List<Pair>(pairs);

            // Nothing to merge
            if (merged.Count < 2)
                return merged.ToArray();

            // This assumes that for the two ranges, low <= high
            for (int i = 0; i < merged.Count - 1; i++)
            {
                Pair low = merged[i];
                Pair high = merged[i + 1];

                // There is some overlap between mn and xy
                // The plus one is if the low set ends one before the high set
                // EX: A-C + D-E => A-E
                if (low.Highest + 1 >= high.Lowest)
                {
                    // The end of the range will be the whatever is higher.
                    char newHighest = low.Highest > high.Highest ? low.Highest : high.Highest;

                    merged[i] = new Pair(low.Lowest, newHighest);
                    merged.RemoveAt(i + 1);

                    // Try to merged the same range with the next range
                    i--;
                }
            }

            return merged.ToArray();
        }

        #endregion



        #region Helper methods        

        private static string GetString(char value) => string.Format("'{0}'", TextUtility.GetAsPrintable(value));

        private static string GetString(Pair pair) => string.Format("('{0}' to '{1}')", TextUtility.GetAsPrintable(pair.Lowest), TextUtility.GetAsPrintable(pair.Highest));

        private static void Swap(ref char a, ref char b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
        #endregion


        // Sets
        #region Sets (only using ASCII for now)
        public static readonly CharacterSet All = CompleteSet.Default;

        public static readonly CharacterSet None = EmptySet.Default;

        public static readonly CharacterSet Null = '\0';

        public static readonly CharacterSet NewLine = List("\n\r");

        public static readonly CharacterSet SpaceOrTab = List(" \t");

        public static readonly CharacterSet Whitespace = SpaceOrTab + NewLine;

        public static readonly CharacterSet Lower = Range('a', 'z');

        public static readonly CharacterSet Upper = Range('A', 'Z');

        public static readonly CharacterSet Letter = Lower + Upper;

        public static readonly CharacterSet LetterOrUnderscore = Letter + '_';

        public static readonly CharacterSet Digit = Range('0', '9');

        public static readonly CharacterSet LetterOrDigit = Letter + Digit;

        public static readonly CharacterSet LetterOrDigitOrUnderscore = LetterOrDigit + '_';

        public static readonly CharacterSet HexDigit = Union(Digit, Range('A', 'F'), Range('a', 'f'));

        public static readonly CharacterSet Symbol = List("!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~");

        public static readonly CharacterSet IdentifierFirst = LetterOrUnderscore;

        public static readonly CharacterSet IdentifierFollow = LetterOrDigitOrUnderscore;

        #endregion
    }
}