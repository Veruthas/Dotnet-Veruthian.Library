using System;
using System.Collections.Generic;
using Veruthian.Library.Text.Runes;

namespace Veruthian.Library.Text
{
    public struct TextLocation : IEquatable<TextLocation>
    {
        readonly int position;

        readonly int line;

        readonly int column;


        public TextLocation(int position, int line, int column)
        {
            this.position = position;
            this.line = line;
            this.column = column;
        }


        public int Position { get => position; }

        public int Line { get => line; }

        public int Column { get => column; }


        public TextLocation IncrementLine(int lengthToEnd = 1)
        {
            return new TextLocation(this.position + lengthToEnd, this.line + 1, 0);
        }


        const uint Null = (uint)'\0';

        const uint Lf = (uint)'\n';

        const uint Cr = (uint)'\r';


        private TextLocation MoveToNextUtf32(uint current, uint next, bool acceptNulls)
        {
            switch (current)
            {
                case Null:
                    return (acceptNulls) ? this + 1 : this;

                case Lf:
                    return this.IncrementLine();

                case Cr:
                    return (next == Lf) ? this + 1 : this.IncrementLine();

                default:
                    return this + 1;
            }
        }

        public TextLocation MoveToNext(Rune current, Rune next, bool acceptNulls = true) => MoveToNextUtf32(current, next, acceptNulls);

        public TextLocation MoveToNext(char current, char next, bool acceptNulls = true) => MoveToNextUtf32(current, next, acceptNulls);


        // MoveThrough Runes
        public TextLocation MoveThrough(Rune current, RuneString following, bool acceptNulls = true)
            => MoveThrough(current, (IEnumerable<Rune>)following, acceptNulls);

        public TextLocation MoveThrough(Rune current, IEnumerable<Rune> following, bool acceptNulls = true)
        {
            TextLocation result = this;

            foreach (Rune next in following)
            {
                result = result.MoveToNext(current, next, acceptNulls);

                current = next;
            }

            return result;
        }

        // MoveThrough Chars
        public TextLocation MoveThrough(char current, string following, bool acceptNulls = true)
            => MoveThrough(current, (IEnumerable<char>)following, acceptNulls);

        public TextLocation MoveThrough(char current, IEnumerable<char> following, bool acceptNulls = true)
        {
            TextLocation result = this;

            foreach (char next in following)
            {
                result = result.MoveToNext(current, next, acceptNulls);

                current = next;
            }

            return result;
        }


        // HashCode
        public override int GetHashCode()
        {
            return HashCodes.Default.Combine(Position, Line, Column);
        }

        // Equals
        public override bool Equals(object obj) => (obj is TextLocation) ? Equals((TextLocation)obj) : false;

        public bool Equals(TextLocation position)
        {
            return (this.position == position.position) &&
                   (this.line == position.line) &&
                   (this.column == position.column);
        }


        public static bool operator ==(TextLocation left, TextLocation right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TextLocation left, TextLocation right)
        {
            return !left.Equals(right);
        }


        public static TextLocation operator +(TextLocation position, int amount)
        {
            return new TextLocation(position.position + amount, position.line, position.column + amount);
        }

        public static TextLocation operator ++(TextLocation position)
        {
            return new TextLocation(position.position + 1, position.line, position.column + 1);
        }


        public override string ToString()
        {
            return string.Format("Position: {0}; Line: {1}; Column: {2}", Position, Line, Column);
        }
    }
}