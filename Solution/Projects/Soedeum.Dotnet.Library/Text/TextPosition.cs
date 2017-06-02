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
            return new TextPosition(this.Position + lengthToEnd, this.Line + 1, 0);
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var position = (TextPosition)obj;

            return this == position;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            int hash = 269;
            hash = (hash * 47) + Position.GetHashCode();
            hash = (hash * 47) + Line.GetHashCode();
            hash = (hash * 47) + Column.GetHashCode();
            return hash;
        }

        public static bool operator ==(TextPosition left, TextPosition right)
        {
            return (left.Position == right.Position) &&
                   (left.Line == right.Line) &&
                   (left.Column == right.Column);
        }

        public static bool operator !=(TextPosition left, TextPosition right)
        {
            return (left.Position != right.Position) ||
                   (left.Line != right.Line) ||
                   (left.Column != right.Column);
        }


        public static TextPosition operator +(TextPosition position, int amount)
        {
            return new TextPosition(position.Position + amount, position.Line, position.Column + amount);
        }

        public static TextPosition operator ++(TextPosition position)
        {
            return new TextPosition(position.Position + 1, position.Line, position.Column + 1);
        }


        public override string ToString()
        {
            return string.Format("Position: {0}; Line: {1}; Column: {2}", Position, Line, Column);
        }
    }
}