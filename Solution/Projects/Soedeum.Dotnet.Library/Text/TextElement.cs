using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Utility;

namespace Soedeum.Dotnet.Library.Text
{
    public struct TextElement : IEquatable<TextElement>
    {
        readonly TextLocation location;
        readonly char value;


        public TextElement(char value)
        {
            this.location = new TextLocation();

            this.value = value;
        }

        public TextElement(TextLocation location, char value)
        {
            this.location = location;

            this.value = value;
        }

        public TextElement(int position, int line, int column, char value)
        {
            this.location = new TextLocation(position, line, column);

            this.value = value;
        }


        public TextLocation Location { get => location; }

        public int Position { get => location.Position; }
        
        public int Line { get => location.Line; }

        public int Column { get => location.Column; }
        
        public char Value { get => value; }


        public bool Is(char value)
        {
            return this.value == value;
        }

        public bool Is(CharSet set)
        {
            return set.Contains(value);
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
            var nextPosition = this.location.MoveToNext(this.value, next);

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
            return string.Format("{0}; Value: '{1}'", this.location, TextUtility.GetAsPrintable(value));
        }


        public bool Equals(TextElement element)
        {
            return this.location == element.location &&
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
            return HashCodeCombiner.Combiner.Combine(location.Position, location.Line, location.Column, value);
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
        public static IEnumerator<TextElement> EnumerateFromChars(TextLocation position, IEnumerator<char> chars, bool acceptNulls = true)
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
            return EnumerateFromChars(new TextLocation(), chars, acceptNulls);
        }

        
        public static IEnumerator<TextElement> EnumerateFromChars(IEnumerable<char> chars, bool acceptNulls = true)
        {
            return EnumerateFromChars(new TextLocation(), chars.GetEnumerator(), acceptNulls);
        }

        public static IEnumerator<TextElement> EnumerateFromChars(TextLocation position, IEnumerable<char> chars, bool acceptNulls = true)
        {
            return EnumerateFromChars(position, chars.GetEnumerator(), acceptNulls);
        }
    }
}