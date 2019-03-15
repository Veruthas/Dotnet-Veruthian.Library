using Veruthian.Library.Types;
using Veruthian.Library.Text.Encodings;
using static Veruthian.Library.Text.Encodings.Utf32;

namespace Veruthian.Library.Text.Lines
{
    public sealed class LineEnding : Enum<LineEnding>
    {
        readonly string description;

        readonly string value;

        readonly int size;


        private LineEnding(string name, string description, string value, int size) : base(name)
        {
            this.description = description;

            this.value = value;

            this.size = size;
        }


        public string Description => description;

        public string Value => value;

        public int Size => size;


        public static bool IsNewLine(uint value, uint next)
        {
            // \r
            if (value == Utf32.Chars.Cr)
                return next != Utf32.Chars.Lf; // only newline if not \r\n
            // \n
            else if (value == Utf32.Chars.Lf)
                return true;
            else
                return false;
        }

        public static LineEnding FromValues(uint value, uint next)
        {
            if (value == Utf32.Chars.Cr)
                return next == Utf32.Chars.Lf ? LineEnding.CrLf : LineEnding.Cr;
            else if (value == Utf32.Chars.Lf)
                return LineEnding.Lf;
            else
                return LineEnding.None;
        }


        public static readonly LineEnding None = new LineEnding("None", "None or End of File", "", 0);

        public static readonly LineEnding Cr = new LineEnding("Cr", "Carriage Return", "\r", 1);

        public static readonly LineEnding Lf= new LineEnding("Lf", "Line Feed", "\n", 1);

        public static readonly LineEnding CrLf= new LineEnding("CrLf", "Carriage Return + Line Feed", "\r\n", 2);

        public static readonly LineEnding CarriageReturn = Cr;

        public static readonly LineEnding LineFeed = Lf ;

        public static readonly LineEnding CarriageReturnLineFeed= CrLf;
    }
}