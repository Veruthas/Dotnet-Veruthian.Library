using System.Text;

namespace Soedeum.Dotnet.Library.Text
{
    public class TextHelper
    {
        public static string GetCharAsPrintable(char value)
        {
            switch (value)
            {
                case '\n':
                    return "\\n";
                case '\r':
                    return "\\r";
                case '\t':
                    return "\\t";
                case '\0':
                    return "\\0";
                default:
                    return value.ToString();
            }
        }

        public static string GetStringAsPrintable(string value)
        {
            var builder = new StringBuilder();

            foreach (char c in value)
                builder.Append(GetCharAsPrintable(c));

            return builder.ToString();
        }

        public static string GetLineEndingAsString(LineEnding ending)
        {
            switch (ending)
            {
                case LineEnding.Null:
                    return "\0";
                case LineEnding.Lf:
                    return "\n";
                case LineEnding.Cr:
                    return "\r";
                case LineEnding.CrLf:
                    return "\r\n";
                default:
                    return "\0";
            }
        }

        public static int GetLineEndingSize(LineEnding ending)
        {
            switch (ending)
            {
                case LineEnding.Null:
                    return 1;
                case LineEnding.Lf:
                case LineEnding.Cr:
                    return 1;
                case LineEnding.CrLf:
                    return 2;
                default:
                    return 0;
            }
        }

        public static LineEnding GetLineEnding(string ending)
        {
            switch (ending)
            {
                case "\n":
                    return LineEnding.Lf;
                case "\r":
                    return LineEnding.Cr;
                case "\r\n":
                    return LineEnding.CrLf;
                case "\0":
                    return LineEnding.Null;
                default:
                    return LineEnding.Null;
            }
        }

        public static string GetLineEndingAsPrintable(LineEnding ending)
        {
            switch (ending)
            {
                case LineEnding.Null:
                    return "\\0";
                case LineEnding.Lf:
                    return "\\n";
                case LineEnding.Cr:
                    return "\\r";
                case LineEnding.CrLf:
                    return "\\r\\n";
                default:
                    return "\\0";
            }
        }
    }
}