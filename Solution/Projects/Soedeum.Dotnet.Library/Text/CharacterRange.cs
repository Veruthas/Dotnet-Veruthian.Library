using System;
using System.Collections;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Utility;

namespace Soedeum.Dotnet.Library.Text
{
    public struct CharacterRange : IEquatable<CharacterRange>, IComparable<CharacterRange>, IEnumerable<char>
    {
        public char Low { get; }

        public char High { get; }

        public CharacterRange(char value)
        {
            Low = High = value;
        }

        public CharacterRange(char low, char high)
        {
            if (low > high)
            {
                this.Low = high;

                this.High = low;
            }
            else
            {
                this.Low = low;

                this.High = high;
            }
        }

        public int Size => (High - Low) + 1;

        public bool ContainsAll() => (Low == char.MinValue) && (High == char.MaxValue);

        public bool Contains(char value) => (value >= Low) && (value <= High);

        public int CompareTo(CharacterRange other)
        {
            if (this.Low < other.Low)
                return -1;
            else if (this.Low > other.Low)
                return 1;
            else if (this.High < other.High)
                return -1;
            else if (this.High > other.High)
                return 1;
            else
                return 0;
        }

        public bool Equals(CharacterRange other) => (this.Low == other.Low) && (this.High == other.High);

        public override bool Equals(object obj) => (obj is CharacterRange) ? Equals((CharacterRange)obj) : false;


        public static bool operator ==(CharacterRange left, CharacterRange right) => left.Equals(right);

        public static bool operator !=(CharacterRange left, CharacterRange right) => !left.Equals(right);

        public static bool operator <(CharacterRange left, CharacterRange right) => left.CompareTo(right) == -1;
        
        public static bool operator >(CharacterRange left, CharacterRange right) => left.CompareTo(right) == 1;


        public override int GetHashCode() => HashCodeCombiner.Combiner.Combine(Low, High);


        public override string ToString()
        {
            if (Size == 1)
                return string.Format("'{0}'", Low.GetAsPrintable());
            else
                return string.Format("('{0}' to '{1}')", Low.GetAsPrintable(), High.GetAsPrintable());
        }

        public IEnumerator<char> GetEnumerator()
        {
            int lowest = Low;
            int highest = High;
            int current = lowest;

            while (current <= highest)
            {
                yield return (char)current;
                current++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();        

        public static implicit operator CharacterRange(char value) => new CharacterRange(value);


        public static int Find(CharacterRange[] sortedSet, char value)
        {
            return -1;
        }
    }
}