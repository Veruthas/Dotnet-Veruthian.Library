using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Soedeum.Dotnet.Library.Utility;

namespace Soedeum.Dotnet.Library.Text
{
    [StructLayout(LayoutKind.Explicit)]
    public struct CharRange : IEquatable<CharRange>, IComparable<CharRange>, IEnumerable<char>
    {
        [FieldOffsetAttribute(0)]
        public readonly char low;


        [FieldOffsetAttribute(2)]
        public readonly char high;


        // Total => bytes: [0-1]:low, [2-3]:high
        // Allows for easy comparison
        [FieldOffsetAttribute(0)]
        private readonly int total;


        public CharRange(char value)
        {
            total = 0;
            low = high = value;
        }

        public CharRange(char low, char high)
        {
            total = 0;

            if (low > high)
            {
                this.low = high;

                this.high = low;
            }
            else
            {
                this.low = low;

                this.high = high;
            }
        }

        public char Low => low;

        public char High => high;

        public int Size => (High - Low) + 1;


        public static readonly int MaxSize = ((int)char.MaxValue) + 1;

        public static readonly CharRange Complete = new CharRange(char.MinValue, char.MaxValue);

        public bool IsComplete => this == Complete;

        public bool IsCharacter => Size == 1;


        public bool Contains(char value) => (value >= Low) && (value <= High);


        public bool Equals(CharRange other) => this.total == other.total;

        public override bool Equals(object obj) => (obj is CharRange) ? Equals((CharRange)obj) : false;

        public static bool operator ==(CharRange left, CharRange right) => left.Equals(right);

        public static bool operator !=(CharRange left, CharRange right) => !left.Equals(right);


        public int CompareTo(CharRange other)
        {
            // if (this.Low < other.Low)
            //     return -1;
            // else if (this.Low > other.Low)
            //     return 1;
            // else if (this.High < other.High)
            //     return -1;
            // else if (this.High > other.High)
            //     return 1;
            // else
            //     return 0;            

            return this.total.CompareTo(other.total);
        }


        public static bool operator <(CharRange left, CharRange right) => left.total < right.total;

        public static bool operator >(CharRange left, CharRange right) => left.total > right.total;


        public override int GetHashCode() => total;


        public override string ToString()
        {
            if (IsCharacter)
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


        public static implicit operator CharRange(char value) => new CharRange(value);


        public static int Find(CharRange[] sortedSet, char value)
        {
            int low = 0;
            int high = sortedSet.Length - 1;

            while (low <= high)
            {
                int current = (high + low) / 2;

                var range = sortedSet[current];

                if (value < range.Low)
                    high = current - 1;
                else if (value > range.High)
                    low = current + 1;
                else
                    return current;
            }

            return -1;
        }

        // Range Operations

        // Assumes a <= b
        // 2 Possible results:
        //  1) Disojoint (a.h < b.l)                    => (a.l to a.h) + (b.l to b.h)
        //  2) Union     (a.l <= b.l) && (a.h <= b.h)   => (a.l to b.h)
        // public static CharacterRangeResult Combine(ref CharacterRange a, ref CharacterRange b)
        // {
        //     if (a.High < b.Low)
        //     {
        //         return CharacterRangeResult.Disjoint;
        //     }
        //     else
        //     {
        //         a = new CharacterRange(a.Low, b.High);

        //         return CharacterRangeResult.Union;
        //     }

        // }

        // Assumes a <= b
        // 7 Possible results
        //   1) Disjoint        (a.h < b.l)                   => (a.l to a.h) + (b.l to b.h)                            -- [0]: A     [1]: B        
        
        //   2) Overlap         (a.l < b.l) && (a.h < b.h)    => (a.l to b.l - 1) + (b.l to a.l) + (a.l + 1 to b.h)     -- [0]: A     [1]: A|B   [2]: B    
        //   3) Contained       (a.l < b.l) && (a.h > b.h)    => (a.l to b.l - 1) + (b.l to b.h) + (b.h + 1 to a.l)     -- [0]: A     [1]: A|B   [2]: A
        //   4) EndContained    (a.l < b.l) && (a.h = b.h)    => (a.l to b.l - 1) + (b.l to a.l)                        -- [0]: A     [1]: A|B  

        //   5) StartContained  (a.l = b.l) && (a.h < b.h)    => (a.l to a.h) + (a.h + 1 to b.l)                        -- [0]: A|B   [1]: B
        //   6) StartOverlap    (a.l = b.l) && (a.h > b.h)    => (a.l to b.h) + (b.h + 1 to a.h)                        -- [0]: A|B   [1]: A
        //   7) Union           (a.l = b.l) && (a.h = b.h)    => (a.l to b.h)                                           -- [0]: A|B        
        // public static CharacterRangeResult Split(ref CharacterRange a, ref CharacterRange b, ref CharacterRange c)
        // {
        //     if (a.High < b.Low)
        //     {
        //         return CharacterRangeResult.Disjoint;
        //     }
        //     else if (a.Low < b.Low)
        //     {

        //     }
        //     // Since a <= b, (a.Low == b.Low)
        //     else
        // }
    }
}