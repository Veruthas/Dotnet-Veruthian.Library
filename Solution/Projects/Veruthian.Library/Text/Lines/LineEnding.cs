using Veruthian.Library.Text.Encodings;
using static Veruthian.Library.Text.Encodings.Utf32;

namespace Veruthian.Library.Text.Lines
{
    public enum LineEnding
    {
        None,

        Cr,

        Lf,

        CrLf,

        NewLine = Lf,

        LineFeed = Lf,

        CarriageReturn = Cr,

        CarriageReturnLineFeed = CrLf,

        EndOfFile
    }

    public static class LineEndings
    {
        public static string GetNewLineString(LineEnding ending)
        {
            switch (ending)
            {
                case LineEnding.Cr:
                    return "\r";
                case LineEnding.Lf:
                    return "\n";
                case LineEnding.CrLf:
                    return "\r\n";
                default:
                    return "";
            }
        }

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

        public static LineEnding GetNewLine(uint value, uint next)
        {
            if (value == Utf32.Chars.Cr)
                return next == Utf32.Chars.Lf ? LineEnding.CrLf : LineEnding.Cr;
            else if (value == Utf32.Chars.Lf)
                return LineEnding.Lf;
            else
                return LineEnding.None;
        }

        public static int GetLineEndingSize(LineEnding ending)
        {
            switch (ending)
            {
                case LineEnding.Cr:
                case LineEnding.Lf:
                    return 1;
                case LineEnding.CrLf:
                    return 2;
                default:
                    return 0;
            }
        }

    }
}