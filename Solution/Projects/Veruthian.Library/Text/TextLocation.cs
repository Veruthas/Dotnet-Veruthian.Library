using System;
using System.Collections.Generic;
using Veruthian.Library.Text.Encodings;
using Veruthian.Library.Text.Runes;
using Veruthian.Library.Utility;

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


        private TextLocation MoveToNextUtf32(uint current, uint next) => Utf32.IsNewLine(current, next) ? this.IncrementLine() : this + 1;


        public TextLocation MoveToNext(Rune current, Rune next) => MoveToNextUtf32(current, next);

        public TextLocation MoveToNext(char current, char next) => MoveToNextUtf32(current, next);


        public TextLocation MoveThrough(Rune current, IEnumerable<Rune> following)
        {
            TextLocation result = this;

            foreach (var next in following)
            {
                result = result.MoveToNext(current, next);

                current = next;
            }

            return result;
        }

        public TextLocation MoveThrough(char current, IEnumerable<char> following)
        {
            TextLocation result = this;

            foreach (var next in following)
            {
                result = result.MoveToNext(current, next);

                current = next;
            }

            return result;
        }


        public static TextLocation operator +(TextLocation position, int amount)
        {
            return new TextLocation(position.position + amount, position.line, position.column + amount);
        }

        public static TextLocation operator ++(TextLocation position)
        {
            return new TextLocation(position.position + 1, position.line, position.column + 1);
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


        // HashCode
        public override int GetHashCode()
        {
            return HashCodes.Default.Combine(Position, Line, Column);
        }


        public override string ToString()
        {
            return string.Format("Position: {0}; Line: {1}; Column: {2}", Position, Line, Column);
        }
    }
}