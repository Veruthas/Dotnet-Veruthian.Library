using Soedeum.Dotnet.Library.Utility;

namespace Soedeum.Dotnet.Library.Text
{
    public struct TextPosition
    {
        readonly int position;

        readonly int line;

        readonly int column;


        public TextPosition(int position, int line, int Column)
        {
            this.position = position;
            this.line = line;
            this.column = Column;
        }


        public int Position { get => position; }

        public int Line { get => line; }

        public int Column { get => column; }


        public TextPosition IncrementLine(int lengthToEnd = 1)
        {
            return new TextPosition(this.position + lengthToEnd, this.line + 1, 0);
        }

        public bool Equals(TextPosition position)
        {
            return (this.position == position.position) &&
                   (this.line == position.line) &&
                   (this.column == position.column);
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var position = (TextPosition)obj;

            return this.Equals(position);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return HashCodeCombiner.Combiner.Combine(Position, Line, Column);
        }

        public static bool operator ==(TextPosition left, TextPosition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TextPosition left, TextPosition right)
        {
            return !left.Equals(right);
        }


        public static TextPosition operator +(TextPosition position, int amount)
        {
            return new TextPosition(position.position + amount, position.line, position.column + amount);
        }

        public static TextPosition operator ++(TextPosition position)
        {
            return new TextPosition(position.position + 1, position.line, position.column + 1);
        }


        public override string ToString()
        {
            return string.Format("Position: {0}; Line: {1}; Column: {2}", Position, Line, Column);
        }
    }
}