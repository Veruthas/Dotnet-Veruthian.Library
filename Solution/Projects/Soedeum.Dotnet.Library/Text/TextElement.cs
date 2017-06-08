using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Utility;

namespace Soedeum.Dotnet.Library.Text
{
    public struct TextElement : IEquatable<TextElement>
    {
        readonly TextPosition position;
        readonly char value;


        public TextElement(char value)
        {
            this.position = new TextPosition();

            this.value = value;
        }

        public TextElement(TextPosition position, char value)
        {
            this.position = position;

            this.value = value;
        }

        public TextElement(int position, int line, int column, char value)
        {
            this.position = new TextPosition(position, line, column);

            this.value = value;
        }


        public TextPosition Position { get => position; }

        public char Value { get => value; }


        public bool Is(char value)
        {
            return this.value == value;
        }

        public bool Is(CharSet set)
        {
            return set.Includes(value);
        }

        public bool IsNull()
        {
            return this.value == '\0';
        }

        public bool IsSpaceOrTab()
        {
            return this.value == ' ' || this.value == '\t';
        }

        public bool IsNewline()
        {
            return this.value == '\n' || this.value == '\r';
        }

        public bool IsWhitespace()
        {
            return char.IsWhiteSpace(this.value);
        }

        public bool IsDigit()
        {
            return char.IsDigit(this.value);
        }

        public bool IsLower()
        {
            return char.IsLower(this.value);
        }

        public bool IsUpper()
        {
            return char.IsUpper(this.value);
        }

        public bool IsLetter()
        {
            return char.IsLetter(this.value);
        }

        public bool IsLetterOrDigit()
        {
            return char.IsLetterOrDigit(this.value);
        }

        public bool IsHexDigit()
        {
            return (this.value >= '0' && this.value <= '9') ||
                   (this.value >= 'A' && this.value <= 'F') ||
                   (this.value >= 'a' && this.value <= 'f');
        }

        public TextElement MoveTo(char next, bool acceptNulls = true)
        {
            var nextPosition = this.Position.MoveToNext(this.value, next);

            return new TextElement(nextPosition, next);
        }

        public TextElement MoveThrough(string following, bool acceptNulls = true)
        {
            TextElement result = this;

            foreach (char next in following)
                result = result.MoveTo(next, acceptNulls);

            return result;
        }

        public override string ToString()
        {
            return string.Format("{0}; Value: '{1}'", position, TextUtility.GetAsPrintable(value));
        }


        public bool Equals(TextElement element)
        {
            return this.position == element.position &&
                   this.value == element.value;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var element = (TextElement)obj;

            return this.Equals(element);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return HashCodeCombiner.Combiner.Combine(position.Position, position.Line, position.Column, value);
        }


        public static implicit operator TextElement(char value)
        {
            return new TextElement(value);
        }


        public static bool operator ==(TextElement left, TextElement right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TextElement left, TextElement right)
        {
            return !left.Equals(right);
        }

        public static TextElement operator +(TextElement left, char right)
        {
            return left.MoveTo(right);
        }

        public static TextElement operator +(TextElement left, string right)
        {
            return left.MoveThrough(right);
        }


        // Enumerators
        public static IEnumerator<TextElement> EnumerateFromChars(TextPosition position, IEnumerator<char> chars, bool acceptNulls = true)
        {
            bool initialized = false;

            TextElement current = default(TextElement);

            foreach (char c in chars.GetEnumerable())
            {
                if (initialized)
                {
                    current.MoveTo(c, acceptNulls);
                }
                else
                {
                    initialized = true;
                    current = c;
                }

                yield return current;
            }
        }

        public static IEnumerator<TextElement> EnumerateFromChars(IEnumerator<char> chars, bool acceptNulls = true)
        {
            return EnumerateFromChars(new TextPosition(), chars, acceptNulls);
        }

        
        public static IEnumerator<TextElement> EnumerateFromChars(IEnumerable<char> chars, bool acceptNulls = true)
        {
            return EnumerateFromChars(new TextPosition(), chars.GetEnumerator(), acceptNulls);
        }

        public static IEnumerator<TextElement> EnumerateFromChars(TextPosition position, IEnumerable<char> chars, bool acceptNulls = true)
        {
            return EnumerateFromChars(position, chars.GetEnumerator(), acceptNulls);
        }
    }
}