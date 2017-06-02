namespace Soedeum.Dotnet.Library.Text
{
    public struct TextElement
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

        public bool IsSpaceOrTab()
        {
            return this.value == ' ' || this.value == '\t';
        }

        public bool IsNewline()
        {
            return this.value == '\n' || this.value == '\r';
        }

        public bool IsNull()
        {
            return this.value == '\0';
        }

        public bool IsWhitespace()
        {
            return char.IsWhiteSpace(this.value);
        }

        public bool IsHexDigit()
        {
            return (this.value >= '0' && this.value <= '9') ||
                   (this.value >= 'A' && this.value <= 'F') ||
                   (this.value >= 'a' && this.value <= 'f');
        }


        public TextElement MoveTo(char value)
        {
            char previous = this.value;

            TextPosition position = this.position;


            switch (previous)
            {
                case '\0':
                    value = '\0';
                    break;

                case '\n':
                    position = position.IncrementLine();
                    break;

                case '\r':
                    position = (value == '\n') ? position + 1 : position = position.IncrementLine();
                    break;

                default:
                    position += 1;
                    break;
            }

            return new TextElement(position, value);
        }

        public TextElement MoveTo(string value)
        {
            TextElement result = this;

            foreach (char letter in value)
                result += value;

            return result;
        }

        public override string ToString()
        {
            return string.Format("{0}; Value: '{1}'", position, TextHelper.GetCharAsPrintable(value));
        }


        // override object.Equals
        public override bool Equals(object obj)
        {
            //
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // TODO: write your implementation of Equals() here
            var texel = (TextElement)obj;

            return this.position == texel.position &&
                    this.value == texel.value;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            int hash = 269;
            hash = (hash * 47) + Position.GetHashCode();
            hash = (hash * 47) + Value.GetHashCode();
            return hash;
        }


        public static implicit operator TextElement(char value)
        {
            return new TextElement(value);
        }


        public static bool operator ==(TextElement left, TextElement right)
        {
            return left.position == right.position &&
                   left.value == right.value;
        }

        public static bool operator !=(TextElement left, TextElement right)
        {
            return left.position != right.position ||
                   left.value != right.value;
        }

        public static TextElement operator +(TextElement left, char right)
        {
            char previous = left.value;

            TextPosition position = left.position;


            switch (previous)
            {
                case '\0':
                    right = '\0';
                    break;

                case '\n':
                    position = position.IncrementLine();
                    break;

                case '\r':
                    position = (right == '\n') ? position + 1 : position = position.IncrementLine();
                    break;

                default:
                    position += 1;
                    break;

            }

            return new TextElement(position, right);
        }

        public static TextElement operator +(TextElement left, string right)
        {
            TextElement result = left;

            foreach (char value in right)
                result += value;

            return result;
        }



    }
}