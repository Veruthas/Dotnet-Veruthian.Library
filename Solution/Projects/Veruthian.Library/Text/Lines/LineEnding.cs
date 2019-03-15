using Veruthian.Library.Types;
using Veruthian.Library.Text.Encodings;
using static Veruthian.Library.Text.Encodings.Utf32;

namespace Veruthian.Library.Text.Lines
{
    public sealed class LineEnding : Enum<LineEnding>
    {
        readonly string abberviated;

        readonly string value;

        readonly int size;


        private LineEnding(string name, string abberivated, string value, int size) : base(name)
        {
            this.abberviated = abberivated;

            this.value = value;

            this.size = size;
        }


        public string AbbreviatedName => abberviated;

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


        public static readonly LineEnding None = new LineEnding("None", "None", "", 0);

        public static readonly LineEnding CarriageReturn = new LineEnding("CarriageReturn", "Cr", "\r", 1);

        public static readonly LineEnding LineFeed = new LineEnding("LineFeed", "Lf", "\n", 1);

        public static readonly LineEnding CarriageReturnLineFeed = new LineEnding("CarriageReturnLineFeed", "CrLf", "\r\n", 2);

        public static readonly LineEnding Cr = CarriageReturn;

        public static readonly LineEnding Lf = LineFeed;

        public static readonly LineEnding CrLf = CarriageReturnLineFeed;
    }
}