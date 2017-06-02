namespace Soedeum.Dotnet.Library.Text
{
 public struct TextPosition
    {
        public TextPosition(int position, int line, int Column)
        {
            this.Position = position;
            this.Line = line;
            this.Column = Column;
        }


        public int Position { get; private set; }

        public int Line { get; private set; }

        public int Column { get; private set; }

        
        public TextPosition IncrementLine(int lengthToEnd = 1)
        {
            var newPosition = this;

            newPosition.Position += lengthToEnd;
            newPosition.Line++;
            newPosition.Column = 0;

            return newPosition;
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
            position.Position += amount;
            position.Column += amount;

            return position;
        }

        public static TextPosition operator ++(TextPosition position)
        {
            position.Position++;
            position.Column++;

            return position;
        }


        public override string ToString()
        {
            return string.Format("Position: {0}; Line: {1}; Column: {2}", Position, Line, Column);
        }
    }
}